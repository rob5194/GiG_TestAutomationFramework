using System;
using System.Collections.Generic;
using System.Text;

namespace TestAutomationFramework.Models
{
    //A response model used to create an instance with response from APIs
    public class CommonResponseModel
    {
        public int StatusCode { get; set; }
        public object Value { get; set; }
        public int Id { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
    }
}
