using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace QueueSubscriber
{
    public class MessageListener //<T> where T:class
    {
        static ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        EventingBasicConsumer consumer;
        readonly Func<string, bool> _callBackActin;
        public MessageListener(string HostIP, Func<string, bool> action)
        {
            _callBackActin = action;
            factory = new ConnectionFactory
            {
                HostName = HostIP
            };
             connection = factory.CreateConnection();
             channel = connection.CreateModel();

            channel.QueueDeclare(queue: "my-queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            consumer = new EventingBasicConsumer(channel);
            consumer.Received += MessageReceived;
            channel.BasicConsume(queue: "my-queue", autoAck: false, consumer: consumer);
        }

        private void MessageReceived(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("Received {0}", message);
            if (string.IsNullOrEmpty(message))
            {
                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            else
            {
                try
                {
                    //process the message
                    _callBackActin.DynamicInvoke(message);
                    //acknowledge
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    //send dont failure message
                    channel.BasicReject(ea.DeliveryTag, requeue: false);
                }
            }
        }

 
        
        /*
       public T Listen<T>(Func<string, T> action, params object[] args)
        {             

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //channel.ExchangeDeclare(exchange: "(AMQP default)", type: "direct", durable:true);
                //var theQueueName = channel.QueueDeclare().QueueName;
                //Console.WriteLine("queue: " + theQueueName);

                channel.QueueDeclare(queue: "my-queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                //channel.QueueBind(queue: theQueueName,
                //                  exchange: "amq.topic",
                //                  routingKey: "*.xlsx");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    //process the message


                    ////acknowledge
                    channel.BasicAck(ea.DeliveryTag, multiple: false);


                    ////if unsuccessful
                    //channel.BasicReject(ea.DeliveryTag, requeue: false);
                };

                //channel.BasicConsume(queue: theQueueName, autoAck: false, consumer: consumer);
                channel.BasicConsume(queue: "my-queue", autoAck: false, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }

        */

    }
}
