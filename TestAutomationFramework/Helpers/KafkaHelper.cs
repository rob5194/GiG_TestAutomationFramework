using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using TestAutomationFramework.Models;

namespace TestAutomationFramework.Helpers
{

    public class KafkaHelper
    {
        jsonClass jsonData = null;

        public Dictionary<string, object> CreateConfig(string prodOrCons)
        {
            jsonData = new jsonClass();

            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd().Replace('.', '_');
                jsonData = JsonConvert.DeserializeObject<jsonClass>(json);
            }
            Dictionary<string, object> config; //new Dictionary<string, object>();

            switch (prodOrCons.ToLower())
            {
                case "producer":
                    // Create the producer configuration
                    config = new Dictionary<string, object>
                    {
                                { "bootstrap.servers", jsonData.bootstrap_servers },
                    };

                    return config; 

                case "consumer":
                    // Create the producer configuration
                    config = new Dictionary<string, object>
                    {
                                { "group.id", jsonData.group_id },
                                { "bootstrap.servers", jsonData.bootstrap_servers },
                                { "auto.commit.interval.ms", jsonData.auto_commit_interval_ms },
                                { "auto.offset.reset", jsonData.auto_offset_reset }
                    };
                    return config;
            }

            return default;
           
        }

        public List<string> CreateProducer(Dictionary<string,object> producerConfig, List<Car> cars)
        {
            List<string> producedMessages = null;

            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                // Send car details to the topic

                foreach(Car c in cars)
                {
                    var message = $"Brand Name: {c.brandName} - Model: {c.model} - Number of Doors: {c.numberOfDoors} - Sportscar: {c.sportsCar}";
                    var result = producer.ProduceAsync(jsonData.topic, null, message).GetAwaiter().GetResult();
                    producedMessages.Add($"{message} on Partition {result.Partition}");
                }
            }

            return producedMessages;
        }

        //public List<string> CreateConsumer(Dictionary<string, object> consumerConfig, List<Car> cars)
        //{
        //    List<string> producedMessages = null;

        //    using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
        //    {
        //        //// Subscribe to the OnMessage event
        //        //consumer.OnMessage += (obj, msg) =>
        //        //{
        //        //    Console.WriteLine($"Received: {msg.Value}");
        //        //};

        //        // Subscribe to the Kafka topic
        //        consumer.Subscribe(new List<string>() { jsonData.topic });

        //        // Subscribe to the OnMessage event
        //        consumer.OnMessage += (obj, msg) =>
        //        {
        //           producedMessages.Add(msg.Value);
        //        };

        //        // Poll for messages
        //        while (!cancelled)
        //        {
        //            consumer.Poll();
        //        }
        //    }
        

            
        //}
    }
}



