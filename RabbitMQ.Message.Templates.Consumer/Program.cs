using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

//EventingBasicConsumer consumer = new(channel);

//channel.BasicConsume(
//    queue: queueName,
//    autoAck: false,
//    consumer: consumer
//    );

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};

#endregion

#region pub/sub
//string exchangeName = "example-pub-sub-exchange";

//channel.ExchangeDeclare(
//    exchange: exchangeName,
//    type: ExchangeType.Fanout
//    );

//string queueName = channel.QueueDeclare().QueueName;
//channel.QueueBind(
//    queue: queueName,
//    exchange: exchangeName,
//    routingKey: string.Empty
//    );

//EventingBasicConsumer consumer = new(channel);

//channel.BasicConsume(
//    queue: queueName,
//    autoAck: false,
//    consumer: consumer
//    );

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};

#endregion

#region work-queue


//string queueName = "example-work-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//EventingBasicConsumer consumer = new(channel);

//channel.BasicConsume(
//    queue: queueName,
//    autoAck: true,
//    consumer: consumer);

//channel.BasicQos(
//    prefetchCount: 1,
//    prefetchSize: 0,
//    global: false);

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};

#endregion

#region request-response

string requestQueueName = "example-request-response-queue";

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

EventingBasicConsumer consumer = new(channel); 
channel.BasicConsume(
    queue: requestQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    byte[] responseMessage = Encoding.UTF8.GetBytes($"İşlem tamamlandı:  {message}");

    IBasicProperties properties = channel.CreateBasicProperties();
    properties.CorrelationId = e.BasicProperties.CorrelationId;
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: e.BasicProperties.ReplyTo,
        basicProperties: properties,
        body: responseMessage);
};

#endregion

Console.Read();



