using System;
using System.Collections.Generic;
using System.Text;

namespace TestAutomationFramework.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        private object _value;
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
