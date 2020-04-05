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
    /// <summary>
    /// A class for configuration and processing of Stream Processing
    /// </summary>
    public class KafkaHelper
    {
        private jsonClass _jsonData = null;
        private int _counter = 0;
        public KafkaHelper()
        {
            _jsonData = new jsonClass();
        }

        /// <summary>
        /// Create Configuration settings for producer or consumer according to parameters
        /// </summary>
        /// <param name="prodOrCons"></param>
        /// <returns></returns>
        public Dictionary<string, object> CreateConfig(string prodOrCons)
        {
            // read configuration data from a JSON file
            using (StreamReader r = new StreamReader("..\\..\\..\\config.json"))
            {
                string json = r.ReadToEnd().Replace('.', '_');
                _jsonData = JsonConvert.DeserializeObject<jsonClass>(json);
            }
            Dictionary<string, object> config; 

            switch (prodOrCons.ToLower())
            {
                case "producer":
                    // Create the producer configuration
                    config = new Dictionary<string, object>
                    {
                                { "bootstrap.servers", _jsonData.bootstrap_servers.Replace('_','.' )}
                    };

                    return config;

                case "consumer":
                    // Create the producer configuration
                    config = new Dictionary<string, object>
                    {
                                { "group.id", _jsonData.group_id },
                                { "bootstrap.servers", _jsonData.bootstrap_servers.Replace('_','.' ) }, 
                                { "enable.auto.commit", true },
                                { "auto.offset.reset", _jsonData.auto_offset_reset }
                    };
                    return config;
            }

            return default;

        }

        /// <summary>
        /// Create a producer based on the settings imported from config.json
        /// </summary>
        /// <param name="producerConfig"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public List<string> CreateProducer(Dictionary<string, object> producerConfig, List<string> messages)
        {
            List<string> producedMessages = new List<string>();

            // Create the producer
            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                try
                {
                    foreach (string msg in messages)
                    {
                        //counter created to check the amount of messages when consuming them
                        _counter++;

                        //Send a message to the topic 
                        producer.ProduceAsync(_jsonData.topic, null, msg).GetAwaiter().GetResult();

                        //add messages to the list of Produced messages
                        producedMessages.Add($"{msg.ToUpper()}");
                    }

                    return producedMessages;
                }

                catch (Exception ex)
                {
                    return null;
                }
            }            
        }

        /// <summary>
        /// Create a consumer based on the settings imported from config.json
        /// </summary>
        /// <param name="consumerConfig"></param>
        /// <returns></returns>
        public List<string> CreateConsumer(Dictionary<string, object> consumerConfig)
        {

            List<string> consumedMessages = new List<string>();
            //create the consumer
            using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
            {
                try
                {
                    // Subscribe to the Kafka topic
                    consumer.Subscribe(new List<string>() { _jsonData.topic });

                    // Subscribe to the OnMessage event
                    consumer.OnMessage += (obj, msg) =>
                    {
                        consumedMessages.Add(msg.Value.ToUpper());

                    };

                    // Poll for messages
                    //Continue the poll until all produced messages are received
                    while (consumedMessages.Count < _counter)
                    {
                        consumer.Poll(100);
                    }

                    // make sure each message is not duplicate
                    return consumedMessages.Distinct().ToList();
                }

                catch (Exception ex)
                {
                    return null;
                }

            }           
        }

    }
}



