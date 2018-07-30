using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using log4net.Repository;

namespace ConsumerApp
{
    public class Consumer
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

        public bool Enabled { get; set; }

        public delegate void OnRecieveMessage(string Message);

        public Consumer()
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

        public void GetMessage()
        {
            connectionFactory = new ConnectionFactory
            {
                Password = Password,
                UserName = UserName,
                HostName = HostName
            };

            connection = connectionFactory.CreateConnection();
            model = connection.CreateModel();
            model.BasicQos(0, 1, false);

            var consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(QueueName, false, consumer);

            while (Enabled)
            {
                //Get next message
                var deliveryArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                logger.Debug("New Message Received");

                //Serialize Message
                Encoding encoding = System.Text.Encoding.UTF8;
                var message = encoding.GetString(deliveryArgs.Body);

                string stroutput = IsValidName(message.ToString());

                logger.Debug(stroutput);

                Console.WriteLine("{0}", stroutput);
                Console.WriteLine();

                model.BasicAck(deliveryArgs.DeliveryTag, false);
            }

        }
        public static string IsValidName(string Name)
        {
            if (Name.Length > 17)
            {
                string strName = Name.Substring(0, 17);
                string strStatic = "Hello my name is,";
                if (strName.Equals(strStatic))
                {
                    return "Hello " + Name.Substring(18) + ",I am your father!";
                }
                else
                {
                    return "Not Valid Message";
                }
            }
            else
            {
                return "Not Valid Message";
            }
        }
    }
}
