using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


ConnectionFactory factory = new();

factory.Uri = new("amqps://nhmssnpr:lCdcfPi66bVVc8bUi0LH76YGN8LeIlrO@shark.rmq.cloudamqp.com/nhmssnpr");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1. Adım
channel.ExchangeDeclare(exchange: "direct-exchange-example",
	type: ExchangeType.Direct);

//2.Adım
string queueName = channel.QueueDeclare().QueueName;

//3. Adım
channel.QueueBind(
	queue: queueName,
	exchange: "direct-exchange-example",
	routingKey: "direct-queue-example");

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
	queue: queueName,
	autoAck: true,
	consumer: consumer);

consumer.Received += (sender, e) =>
{
	string message = Encoding.UTF8.GetString(e.Body.Span);
	Console.WriteLine(message);
};

Console.Read();

//1. Adım: Publisher'daki exchange ile birebir aynı isim ve type'a sahip bir exchange tanımla

//2. Adım: Publisher tarafından routing key'de bulunan değerdeki kuyruğa gönderilen mesajları 
// kendi oluşturduğumuz kuyruğa yönlendirerek tüketmemiz gerekir. Bundan dolayı kuyruk oluştururuz.

//3. Adım: 