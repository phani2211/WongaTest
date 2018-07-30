using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace ConsumerApp
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
                Console.WriteLine("RabbitMQ Consumer");
                Console.WriteLine();
                logger.Debug("Starting RabbitMQ Consumer");

                var queue = new Consumer() { Enabled = true };
                queue.GetMessage();
                logger.Debug("Ending RabbitMQ Consumer");
                Console.ReadLine();               
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
        }
    }
}