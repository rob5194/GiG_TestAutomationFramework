using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        int counter = 0;
        public KafkaHelper()
        {
            jsonData = new jsonClass();
        }

        public Dictionary<string, object> CreateConfig(string prodOrCons)
        {

            using (StreamReader r = new StreamReader("..\\..\\..\\config.json"))
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
                                { "bootstrap.servers", jsonData.bootstrap_servers.Replace('_','.' )} ////127.0.0.1:9092 },
                    };

                    return config;

                case "consumer":
                    // Create the producer configuration
                    config = new Dictionary<string, object>
                    {
                                { "group.id", jsonData.group_id },// "myconsumer" },//jsonData.group_id },
                                { "bootstrap.servers", jsonData.bootstrap_servers.Replace('_','.' ) }, //"127.0.0.1:9092" },//jsonData.bootstrap_servers },
                                { "enable.auto.commit", true },
                                { "auto.offset.reset", jsonData.auto_offset_reset }//"earliest" }// }jsonData.auto_offset_reset }
                    };
                    return config;
            }

            return default;

        }

        public List<string> CreateProducer(Dictionary<string, object> producerConfig, List<Car> cars)
        {

            List<string> producedMessages = new List<string>();

            // Create the producer
            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {

                foreach (Car c in cars)
                {
                    counter++;
                    var message = $"Brand Name: {c.brandName} | Model: {c.model} | Doors: {c.numberOfDoors} | Sports: {c.sportsCar}";
                    var result = producer.ProduceAsync(jsonData.topic, null, message).GetAwaiter().GetResult();
                    producedMessages.Add($"{message.ToUpper()}");
                }
            }

            return producedMessages;
        }

        public List<string> CreateConsumer(Dictionary<string, object> consumerConfig)
        {
            List<string> producedMessages = new List<string>();
            Stopwatch s = new Stopwatch();
            s.Start();
            using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
            {
                try
                {
                    // Subscribe to the Kafka topic
                    consumer.Subscribe(new List<string>() { jsonData.topic });
                    // Subscribe to the OnMessage event
                    consumer.OnMessage += (obj, msg) =>
                    {
                        producedMessages.Add(msg.Value.ToUpper());

                    };

                    // Poll for messages
                    while (producedMessages.Count < counter)
                    {
                        consumer.Poll(100);

                    }

                    //consumer.Dispose();
                    return producedMessages.Distinct().ToList();
                }

                catch (Exception ex)
                {
                    return null;
                }

            }           
        }

    }
}

//Stopwatch s = new Stopwatch();
//s.Start();
//                    while (s.Elapsed<TimeSpan.FromSeconds(10))
//                    {
//                        consumer.Poll();
//                    }



