using RabbitMQ.Client;
using System.Text;

//Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://nhmssnpr:lCdcfPi66bVVc8bUi0LH76YGN8LeIlrO@shark.rmq.cloudamqp.com/nhmssnpr");


//Bağlantı aktifleştirme ve kanal açma
using IConnection connection =  factory.CreateConnection();
using IModel channel =  connection.CreateModel();


//Queue oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive:false, durable: true);

//Queue'ya mesaj gönderme

//RabbitMQ kuyruğa atacağı mesajları byte türünden kabul etmektedir. Bu yüzden mesajları byte türüne dönüştürmemiz gerekir

IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

for (int i = 0; i < 10; i++)
{
	await Task.Delay(200);
	byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
	channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties:properties);
}

Console.Read();