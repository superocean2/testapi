using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class IncomeExpenseJson
    {
        public IncomeExpenseJson()
        {
            incomes = new List<Income>();
            expenses = new List<Expense>();
            categories = new List<Category>();
        }
        public string Response { get; set; }
        public List<Income> incomes { get; set; }
        public List<Expense> expenses { get; set; }
        public List<Category> categories { get; set; }
    }
}