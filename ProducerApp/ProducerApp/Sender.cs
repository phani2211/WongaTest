using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using log4net.Repository;

namespace ProducerApp
{
    public class Sender
    {
        private ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "NewQueue";
        private const string ExchangeName = "NewExchange";
        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel model;

        log4net.ILog logger = LogManager.GetLogger(typeof(Program));

        public Sender()
        {
            Display();
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        private void Display()
        {
            Console.WriteLine("Host:{0}", HostName);
            logger.Debug("Host: " + HostName);
            Console.WriteLine("UserName:{0}", UserName);
            logger.Debug("UserName: " + UserName);
            Console.WriteLine("Password:{0}", Password);
            logger.Debug("Password: " + Password);
            Console.WriteLine("QueueName:{0}", QueueName);
            logger.Debug("QueueName: " + QueueName);
            Console.WriteLine("ExchangeName:{0}", ExchangeName);
            logger.Debug("ExchangeName: " + ExchangeName);
            Console.WriteLine();
        }

        public void Send(string message)
        {
            connectionFactory = new ConnectionFactory
            {
                Password = Password,
                UserName = UserName,
                HostName = HostName
            };

            connection = connectionFactory.CreateConnection();
            model = connection.CreateModel();

            model.QueueDeclare(QueueName, true, false, false, null);
            Console.WriteLine("Created a Queue");
            Console.WriteLine();

            model.ExchangeDeclare(ExchangeName, ExchangeType.Topic);
            Console.WriteLine("Created an Exchange");
            Console.WriteLine();

            model.QueueBind("NewQueue", "NewExchange", "RoutingKey");
            Console.WriteLine("Exchange and Queue bound");
            logger.Debug("Exchange and Queue bound");
            Console.WriteLine();


            var properties = model.CreateBasicProperties();
            properties.SetPersistent(true);

            Encoding encoding = System.Text.Encoding.UTF8;
            byte[] messageBuffer = encoding.GetBytes(message);
            Console.WriteLine("Message Serialized");
            logger.Debug("Message Serialized");
            Console.WriteLine();

            //Send Message
            model.BasicPublish(ExchangeName, "RoutingKey", properties, messageBuffer);
            Console.WriteLine("Message sent successfully");
            logger.Debug("Message sent successfully");
            Console.WriteLine();
        }

    }
}
