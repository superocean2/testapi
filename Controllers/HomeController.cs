using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using API.Models;
using System.Web.Script.Serialization;

namespace API.Controllers
{
    public class HomeController : Controller
    {
        DatabaseContext db = new Models.DatabaseContext();

        public string Test()
        {
            return "OK";
        }

        // GET: Home
        public string GetData()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            IncomeExpenseJson incomeExpenses = new Models.IncomeExpenseJson();
            incomeExpenses.categories.AddRange(db.Categories);
            incomeExpenses.incomes.AddRange(db.Incomes);
            incomeExpenses.expenses.AddRange(db.Expenses);
            incomeExpenses.Response = "ok";
            return js.Serialize(incomeExpenses);
        }

        //POST: Data
        [HttpPost]
        public void PostData()
        {
            string incomesText = Request.Form["income"].ToString().Trim();
            string expensesText = Request.Form["expense"].ToString().Trim();
            string categoriesText = Request.Form["category"].ToString().Trim();
            string username = Request.Form["username"].ToString().Trim();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<Income> phoneIncomes = (List<Income>)js.Deserialize(incomesText, typeof(List<Income>));
            List<Expense> phoneExpenses = (List<Expense>)js.Deserialize(expensesText, typeof(List<Expense>));
            List<Category> phoneCategories = (List<Category>)js.Deserialize(categoriesText, typeof(List<Category>));

            List<Income> serverIncomes = db.Incomes.Where(c => c.username == username).ToList();
            List<Expense> serverExpenses = db.Expenses.Where(c => c.username == username).ToList();
            List<Category> serverCategories = db.Categories.Where(c => c.username == username).ToList();

            //category
            if (serverCategories.Count == 0)
            {
                db.Categories.AddRange(phoneCategories.Where(c => c.isdelete == false));
                db.SaveChanges();
            }
            else
            {
                foreach (var item in phoneCategories)
                {

                    if (serverCategories.Any(c => c.categoryid == item.categoryid))
                    {
                        Category serverCat = serverCategories.Find(c => c.categoryid == item.categoryid);
                        if (item.isdelete)
                        {
                            if (!serverCat.isdelete)
                            {
                                serverCat.isdelete = true;
                                db.SaveChanges();
                            }

                        }
                        else
                        {
                            if (!serverCat.isdelete)
                            {
                                if (item.version > serverCat.version)
                                {
                                    db.Categories.Remove(serverCat);
                                    db.SaveChanges();
                                    db.Categories.Add(item);
                                    db.SaveChanges();
                                }
                            }

                        }

                    }
                    else
                    {
                        db.Categories.Add(item);
                        db.SaveChanges();
                    }
                }

            }

            //income
            if (serverIncomes.Count == 0)
            {
                db.Incomes.AddRange(phoneIncomes.Where(c => c.isdelete == false));
                db.SaveChanges();
            }
            else
            {
                foreach (var item in phoneIncomes)
                {

                    if (serverIncomes.Any(c => c.incomeid == item.incomeid))
                    {
                        Income serverIncome = serverIncomes.Find(c => c.incomeid == item.incomeid);
                        if (item.isdelete)
                        {
                            if (!serverIncome.isdelete)
                            {
                                serverIncome.isdelete = true;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (!serverIncome.isdelete)
                            {
                                if (item.version > serverIncome.version)
                                {
                                    db.Incomes.Remove(serverIncome);
                                    db.SaveChanges();
                                    db.Incomes.Add(item);
                                    db.SaveChanges();
                                }
                            }

                        }

                    }
                    else
                    {
                        db.Incomes.Add(item);
                        db.SaveChanges();
                    }

                }
            }

            //expense
            if (serverExpenses.Count == 0)
            {
                db.Expenses.AddRange(phoneExpenses.Where(c => c.isdelete == false));
                db.SaveChanges();
            }
            else
            {
                foreach (var item in phoneExpenses)
                {

                    if (serverExpenses.Any(c => c.expenseid == item.expenseid))
                    {
                        Expense serverExpense = serverExpenses.Find(c => c.expenseid == item.expenseid);

                        if (item.isdelete)
                        {
                            if (!serverExpense.isdelete)
                            {
                                serverExpense.isdelete = true;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (!serverExpense.isdelete)
                            {
                                if (item.version > serverExpense.version)
                                {
                                    db.Expenses.Remove(serverExpense);
                                    db.SaveChanges();
                                    db.Expenses.Add(item);
                                    db.SaveChanges();
                                }
                            }
                            
                        }

                    }
                    else
                    {
                        db.Expenses.Add(item);
                        db.SaveChanges();
                    }
                }

            }

        }


        //POST: Register
        [HttpPost]
        public void Register()
        {
            String username = Request.Form["username"].ToString().Trim();
            String password = Request.Form["password"].ToString().Trim();
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                if (db.Users.Any(c => c.username == username))
                {
                    throw new Exception();
                }
                else
                {
                    Models.User user = new Models.User { username = username, password = password };
                    db.Users.Add(user);
                    db.SaveChanges();
                }

            }
        }

        //GET: Login
        public string Login()
        {
            String username = "";
            String password = "";
            if (Request.QueryString["username"] != null && Request.QueryString["password"] != null)
            {
                username = Request.QueryString["username"].ToString();
                password = Request.QueryString["password"].ToString();
            }

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                User user = db.Users.FirstOrDefault(c => c.username == username);
                if (user != null)
                {
                    if (user.password == password)
                    {
                        return "ok";
                    }
                }
            }
            return "error";
        }

    }
}