using RabbitMQ.Client;
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
string exchangeName = "example-pub-sub-exchange";

channel.ExchangeDeclare(
    exchange: exchangeName,
    type: ExchangeType.Fanout
    );

for (int i = 0; i < 10; i++)
{

    await Task.Delay(200);

    byte[] message = Encoding.UTF8.GetBytes("mergaba" + i);

    channel.BasicPublish(
        exchange: exchangeName,
        routingKey: string.Empty,
        body: message
        );
}

#endregion


Console.Read();
