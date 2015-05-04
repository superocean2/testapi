using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Category
    {
        public long id { get; set; }
        public string name { get; set; }
        public bool isincome { get; set; }
        public string username { get; set; }
        public string categoryid { get; set; }
        public bool isdelete { get; set; }
        public int version { get; set; }
    }
}