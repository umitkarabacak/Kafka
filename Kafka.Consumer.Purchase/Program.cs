using Confluent.Kafka;
using Kafka.Domain;
using Kafka.Domain.Models.Mail;
using System;
using System.Text.Json;
using System.Threading;

namespace Kafka.Consumer.Purchase
{
    public class Program
    {
        private static void Main(string[] args)
        {
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
                consumer.Subscribe(AppConsts.TopicNamePurchase);
                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cts.Token);

                        var consumeDataKey = Guid.Parse(cr.Message.Key);
                        Console.WriteLine($"Consume Key:\t {JsonSerializer.Serialize(consumeDataKey)} \n\n");

                        var consumeDataValue = JsonSerializer.Deserialize<SendMailRequest>(cr.Message.Value);
                        Console.WriteLine($"Consume Data:\t {JsonSerializer.Serialize(consumeDataValue)} \n\n");
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
