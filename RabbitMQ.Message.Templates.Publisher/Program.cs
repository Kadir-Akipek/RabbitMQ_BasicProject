using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("\"amqps://nhmssnpr:lCdcfPi66bVVc8bUi0LH76YGN8LeIlrO@shark.rmq.cloudamqp.com/nhmssnpr\"");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P
//string queueName = "example-p2p-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//byte[] message = Encoding.UTF8.GetBytes("merhaba");
//channel.BasicPublish(
//    exchange: string.Empty,
//    routingKey: queueName,
//    body: message
//    );

#endregion

#region pub/sub
//string exchangeName = "example-pub-sub-exchange";

//channel.ExchangeDeclare(
//    exchange: exchangeName,
//    type: ExchangeType.Fanout
//    );

//for (int i = 0; i < 10; i++)
//{

//    await Task.Delay(200);

//    byte[] message = Encoding.UTF8.GetBytes("mergaba" + i);

//    channel.BasicPublish(
//        exchange: exchangeName,
//        routingKey: string.Empty,
//        body: message
//        );
//}

#endregion 

#region work-queue

//string queueName = "example-work-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//for (int i = 0; i < 10; i++)
//{

//    await Task.Delay(200);

//    byte[] message = Encoding.UTF8.GetBytes("mergaba" + i);

//    channel.BasicPublish(
//        exchange: string.Empty,
//        routingKey: queueName,
//        body: message
//        );
//}

#endregion

#region request-response

string requestQueueName = "example-request-response-queue";

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

string replyQueueName = channel.QueueDeclare().QueueName;

string correlationId = Guid.NewGuid().ToString();

//Request mesajını oluşturma e gönderme

IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("merhaba" + i);
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        body: message,
        basicProperties: properties);
}

//Response Kuyruğu Dinleme

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        Console.WriteLine($"response : {Encoding.UTF8.GetString(e.Body.Span)}");
    }
};

#endregion



Console.Read();
