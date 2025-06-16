using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://nhmssnpr:lCdcfPi66bVVc8bUi0LH76YGN8LeIlrO@shark.rmq.cloudamqp.com/nhmssnpr");


//Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


//Queue oluşturma
channel.QueueDeclare();
//Consumer'dada kuyruk publisher'daki ile birebir aynı yapılandırmayla tanımlanmalıdır.
channel.QueueDeclare(queue: "example-queue", exclusive: false);


//Queue'den mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", false, consumer);

consumer.Received += (sender, e) =>
{
	//kuyruğa gelen mesajın işlendiği yer
	//e.Body : Kuyruktaki mesajın verisini bütünsel olarak getirecektir
	//e.Body.Span veya e.Body.ToArray() : kuyruktaki mesajın byte verisini getirecektir
	Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.Read();