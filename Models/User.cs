using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class User
    {
        public long id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}