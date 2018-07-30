using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;


namespace ProducerApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            var logger = LogManager.GetLogger(typeof(Program));

            try
            {
                Console.WriteLine("RabbitMQ Sender");
                Console.WriteLine();
                logger.Debug("Starting RabbitMQ Sender");

                var sender = new Sender();

                Console.WriteLine("Please Enter Name:");               
                string strMessage = Console.ReadLine();
                logger.Debug("Please Enter Name:" + strMessage);

                sender.Send("Hello my name is, " + strMessage);
                logger.Debug("Ending RabbitMQ Sender");

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
        }
    }
}