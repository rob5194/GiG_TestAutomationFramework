using System;
using System.Collections.Generic;
using System.Text;

namespace TestAutomationFramework.Models
{
    public class Car
    {
        public string brandName { get; set; }
        public string model { get; set; }
        public int numberOfDoors { get; set; }
        public char sportsCar { get; set; }

        public Car(string brandName, string model, int numberOfDoors, char sportsCar)
        {
            this.brandName = brandName;
            this.model = model;
            this.numberOfDoors = numberOfDoors;
            this.sportsCar = sportsCar;
        }
    }
}
