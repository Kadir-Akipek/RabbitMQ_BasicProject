using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();

factory.Uri = new("amqps://nhmssnpr:lCdcfPi66bVVc8bUi0LH76YGN8LeIlrO@shark.rmq.cloudamqp.com/nhmssnpr");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
	Console.Write("Mesaj: ");
	string message = Console.ReadLine();
	byte[] byteMessage = Encoding.UTF8.GetBytes(message);

	channel.BasicPublish(
		exchange: "direct-exchange-example",
		routingKey: "direct-queue-example",
		body: byteMessage);
}

Console.Read();

