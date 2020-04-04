using System;
using System.Collections.Generic;
using System.Text;

namespace TestAutomationFramework.Models
{
    public class ResponseModel
    {
        public int statusCode { get; set; }
        public object value { get; set; }

        public string token { get; set; }

    }
}
