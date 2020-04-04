using System;
using System.Collections.Generic;
using System.Text;

namespace TestAutomationFramework.Models
{
    public class CommonResponseModel
    {
        public int StatusCode { get; set; }
        public object Value { get; set; }
        public int Id { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
    }
}
