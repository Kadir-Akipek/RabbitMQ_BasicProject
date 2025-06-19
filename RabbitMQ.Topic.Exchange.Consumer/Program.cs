using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://nhmssnpr:lCdcfPi66bVVc8bUi0LH76YGN8LeIlrO@shark.rmq.cloudamqp.com/nhmssnpr");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
	exchange: "topic-exchange-example",
	type: ExchangeType.Topic
	);

Console.Write("Dinlenecek topic formatını belirtiniz: ");
string topic = Console.ReadLine();
string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(
	queue:  queueName,
	exchange: "topic-exchange-example",
	routingKey: topic
	);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
	queue: queueName,
	autoAck: true,
	consumer
	);

consumer.Received += (sender, e) =>
{
	string message = Encoding.UTF8.GetString(e.Body.Span);
	Console.WriteLine(message);
};

Console.Read();



