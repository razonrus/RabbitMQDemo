using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitConsumer
{
    class Program
    {
        private const string queueName = "myQueue";

        static void Main()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using (IConnection connection = connectionFactory.CreateConnection())
            using (IModel chanell = connection.CreateModel())
            {
                chanell.QueueDeclare(queueName, false, false, false, null);

                var consumer = new QueueingBasicConsumer(chanell);
                chanell.BasicQos(0, 1, false);
                //true
                chanell.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    var message = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    var body = Encoding.UTF8.GetString(message.Body);

                    Thread.Sleep(body.Length * 1000);
                    Console.WriteLine("Received message: " + body);
                    
                    chanell.BasicAck(message.DeliveryTag, false);
                }
            }
        }
    }
}
