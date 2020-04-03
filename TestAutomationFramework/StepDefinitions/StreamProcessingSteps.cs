using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TestAutomationFramework.Helpers;
using TestAutomationFramework.Models;

namespace TestAutomationFramework.StepDefinitions
{
    [Binding]
    public class StreamProcessingSteps
    {
        List<Car> cars = new List<Car>();
        KafkaHelper kh = new KafkaHelper();
        List<string> givenMessage = new List<string>();
        List<string> expectedMessage = new List<string>();
        //public StreamProcessingSteps()
        //{
        //    cars.Add(new Car { brandName = "Volkswagen", model = "Beetle", numberOfDoors = 2, sportsCar = 'N' });
        //}

        [Given(@"I have produced some messages to a '(.*)'")]
        public void GivenIHaveProducedSomeMessagesToA(string p0)
        {

            Car vw = new Car("Wolksvagen", "Passat", 2, 'N');
            Car ford = new Car("Ford", "Focus", 4, 'N');
            Car fiat = new Car("Fiat", "500", 2, 'Y');
            cars.Add(vw);
            cars.Add(ford);
            cars.Add(fiat);
            p0 = "producer";
            Dictionary<string, object> prod = kh.CreateConfig(p0);
            givenMessage = kh.CreateProducer(prod, cars);
        }
        
        [Then(@"I expect a list of messages from a '(.*)'")]
        public void ThenIExpectAListOfMessagesFromA(string p0)
        {
            p0 = "consumer";
            Dictionary<string, object> cons = kh.CreateConfig(p0);
            expectedMessage = kh.CreateConsumer(cons);
        }
        
        [Then(@"I expect that messages are delivered correctly")]
        public void ThenIExpectThatMessagesAreDeliveredCorrectly()
        {
              foreach(String givenMes in givenMessage)
            {
                bool contained = expectedMessage.Any(l => l.Contains(givenMes));
                Assert.AreEqual(true, contained);
            }
        }
    }
}
