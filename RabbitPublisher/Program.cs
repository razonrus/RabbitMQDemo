using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitPublisher
{
    internal class Program
    {
        private const string queueName = "myQueue";

        private static void Main()
        {
            var connectionFactory = new ConnectionFactory
                                        {
                                            HostName = "localhost"
                                        };

            while (true)
            {
                var message = Console.ReadLine();

                using (IConnection connection = connectionFactory.CreateConnection())
                using (IModel chanell = connection.CreateModel())
                {
                    chanell.QueueDeclare(queueName, false, false, false, null);

                    byte[] payload = Encoding.UTF8.GetBytes(message ?? "empty string");
                    chanell.BasicPublish("", queueName, chanell.CreateBasicProperties(), payload);
                    Console.WriteLine("Sent message");
                }
            }
        }
    }
}