using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Expense
    {
        public long id { get; set; }
        public string categoryid {get;set;}
        public double amount { get; set; }
        public String date { get; set; }
        public String hour { get; set; }
        public string username { get; set; }
        public String description { get; set; }
        public string expenseid { get; set; }
        public bool isdelete { get; set; }
        public int version { get; set; }
    }
}