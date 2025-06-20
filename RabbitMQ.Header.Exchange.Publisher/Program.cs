using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://nhmssnpr:lCdcfPi66bVVc8bUi0LH76YGN8LeIlrO@shark.rmq.cloudamqp.com/nhmssnpr");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
	exchange: "header-exchange-example",
	type: ExchangeType.Headers);

for (int i = 0; i < 10; i++)
{
	await Task.Delay(200);
	byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
	Console.Write("Header value'sunu giriniz: ");
	string value = Console.ReadLine();

	IBasicProperties basicProperties =  channel.CreateBasicProperties();
	basicProperties.Headers = new Dictionary<string, object>
	{
		["no"] = value
	};

	channel.BasicPublish(
		exchange: "header-exchange-example",
		routingKey: string.Empty,
		body: message,
		basicProperties: basicProperties);
};

Console.Read();




