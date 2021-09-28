using Confluent.Kafka;
using Kafka.Producer.Domain.Models.Mail;
using System;
using System.Text.Json;
using System.Threading;

namespace Kafka.Consumer.Purchase
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string topic = "purchases";
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "kafka-dotnet-getting-started",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
            {
                consumer.Subscribe(topic);
                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cts.Token);

                        var consumeDataKey = Guid.Parse(cr.Message.Key);
                        Console.WriteLine(JsonSerializer.Serialize(consumeDataKey));

                        var consumeDataValue = JsonSerializer.Deserialize<SendMailRequest>(cr.Message.Value);
                        Console.WriteLine(JsonSerializer.Serialize(consumeDataValue));
                    }
                }
                catch (OperationCanceledException operationCanceledException)
                {
                    Console.WriteLine(JsonSerializer.Serialize(operationCanceledException));
                }
                finally
                {
                    consumer.Close();
                }
            }

            Console.ReadKey();
        }
    }
}
