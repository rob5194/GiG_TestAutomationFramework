using System;
using System.Collections.Generic;
using System.Text;

namespace TestAutomationFramework.Models
{
    /// <summary>
    /// A Data class to store data related to a particular Data of the user
    /// </summary>
    public class Data
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Avatar { get; set; }
    }

    /// <summary>
    /// A user class to store data on a User
    /// </summary>
    public class Users
    {
        public int Page { get; set; }
        public int Per_Page { get; set; }
        public int Total { get; set; }
        public int Total_Pages { get; set; }
        public List<Data> Data { get; set; }
    }
}
