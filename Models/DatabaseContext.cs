using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext()
        {
            System.Data.Entity.Database.DefaultConnectionFactory = new SqLiteConnectionFactory();
            CheckDB();
        }
        public DatabaseContext(string connectionString)
            : base(connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
            
        }
        public class SqLiteConnectionFactory : IDbConnectionFactory
        {
            public System.Data.Common.DbConnection CreateConnection(string nameOrConnectionString)
            {
                var databaseDirectory = HttpContext.Current.Server.MapPath("~/data.sqlite");

                var builder = new SQLiteConnectionStringBuilder
                {
                    DataSource = databaseDirectory,
                    Version = 3
                };
                return new SQLiteConnection(builder.ToString());
            }
        }

        public DB db = new DB ();
        public DbSet<Category> Categories { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }
        private void CheckDB()
        {

            db.UpdateTry("create table if not exists \"Categories\" (\"id\"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,\"name\"  TEXT,\"isincome\"  INTEGER,\"username\"  TEXT,\"categoryid\"  TEXT,\"isdelete\"  INTEGER,\"version\"  INTEGER);");
            db.UpdateTry("create table if not exists \"Incomes\" (\"id\"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,\"categoryid\"  TEXT,\"amount\"  FLOAT,\"date\"  TEXT,\"hour\"  TEXT,\"username\"  TEXT,\"description\"  TEXT,\"incomeid\"  TEXT,\"isdelete\"  INTEGER,\"version\"  INTEGER);");
            db.UpdateTry("create table if not exists \"Expenses\" (\"id\"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,\"categoryid\"  TEXT,\"amount\"  FLOAT,\"date\"  TEXT,\"hour\"  TEXT,\"username\"  TEXT,\"description\"  TEXT,\"expenseid\"  TEXT,\"isdelete\"  INTEGER,\"version\"  INTEGER);");
            db.UpdateTry("create table if not exists \"Users\" (\"id\"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,\"username\"  TEXT,\"password\"  TEXT);");

        }
    }
}