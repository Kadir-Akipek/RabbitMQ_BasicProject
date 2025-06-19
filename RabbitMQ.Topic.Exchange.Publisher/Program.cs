using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://nhmssnpr:lCdcfPi66bVVc8bUi0LH76YGN8LeIlrO@shark.rmq.cloudamqp.com/nhmssnpr");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
	exchange:"topic-exchange-example",
	type: ExchangeType.Topic
	);

for (int i = 0; i < 10; i++)
{
	await Task.Delay(200);
	byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
	Console.Write("Mesajın gönderileceği topic formatını belirtiniz: ");
	string topic = Console.ReadLine();
	channel.BasicPublish(
		exchange: "topic-exchange-example",
		routingKey: topic,
		body: message
		);
};

Console.Read();



