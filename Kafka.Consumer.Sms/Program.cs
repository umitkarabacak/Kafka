using Confluent.Kafka;
using Kafka.Domain;
using System;
using System.Text.Json;
using System.Threading;

namespace Kafka.Consumer.Sms
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "SMS",
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
                consumer.Subscribe(AppConsts.TopicNameSms);
                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cts.Token);

                        var consumeDataKey = cr.Message.Key;
                        Console.WriteLine(JsonSerializer.Serialize(consumeDataKey));

                        var consumeDataValue = cr.Message.Value;
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
