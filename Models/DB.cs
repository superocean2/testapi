using System;
using System.Data;
using System.Web;

namespace API.Models
{
    /// <summary>
    /// Klasse for forbindelse til databasen
    /// </summary>
    /// <remarks></remarks>
    public class DB
    {

        public string Database
        {
            get
            {
                if (_database == null) _database = HttpContext.Current.Server.MapPath("~/data.sqlite");
                return _database;
            }
            set
            {
                _database = value;
            }
        }
        private string _database = null;

        // Get or set the connection string to the SQLite database
        private string ConnectionString
        {
            get
            {
                return "Data Source=" + Database + ";Version=3;";
                //"PRAGMA journal_mode=WAL;Pooling=false;";
            }
        }

        public bool Create()
        {
            if (System.IO.File.Exists(Database)) return false;
            System.Data.SQLite.SQLiteConnection.CreateFile(Database);
            return true;
        }

        public bool CreateTry()
        {
            try
            {
                return Create();
            }
            catch
            {
                return false;
            }
        }
        /// <summary>Funksjon for å returnere data fra databasen</summary>
        public System.Data.DataTable LoadTry(string SQL)
        {
            try
            {
                return Load(SQL);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Funksjon for å returnere data fra databasen</summary>
        public System.Data.DataTable Load(string SQL)
        {
            DataSet ds = new DataSet();
            using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(ConnectionString, true))
            {
                connection.Open();
                using (System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(SQL, connection))
                {
                    using (System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(command))
                    {
                        da.Fill(ds);
                    }
                }
                connection.Close();
            }
            if (ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        /// <summary>Save data in database</summary>
        public bool Update(string SQL)
        {

            System.Data.SQLite.SQLiteConnection connection = null;

            if (cnn != null)
            {
                connection = cnn;
            }
            else
            {
                connection = new System.Data.SQLite.SQLiteConnection(ConnectionString, true);
                connection.Open();
            }

            using (System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(SQL, connection))
            {
                command.ExecuteNonQuery();
            }

            if (cnn != null)
            {
                connection = null;
            }
            else
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }

            return true;
        }

        /// <summary>Save data in database</summary>

        private System.Data.SQLite.SQLiteTransaction transaction = null;
        private System.Data.SQLite.SQLiteConnection cnn = null;

        public bool StartTransaction()
        {
            try
            {
                cnn = new System.Data.SQLite.SQLiteConnection(ConnectionString, true);
                cnn.Open();
                transaction = cnn.BeginTransaction();
            }
            catch
            {
                transaction = null;
                if (cnn != null)
                {
                    if (cnn.State == ConnectionState.Open) cnn.Close();
                    cnn = null;
                }
                return false;
            }
            return true;
        }

        public bool StopTransaction()
        {
            transaction.Commit();
            transaction.Dispose();
            transaction = null;
            cnn.Close();
            cnn.Dispose();
            cnn = null;
            return true;
        }

        public bool ClearTransaction()
        {
            if (transaction != null) transaction.Dispose();
            transaction = null;
            if (cnn != null) cnn.Close();
            if (cnn != null) cnn.Dispose();
            cnn = null;
            return true;
        }


        /// <summary>Save data in database</summary>
        public bool UpdateTry(string SQL)
        {
            try
            {
                return Update(SQL);
            }
            catch
            {
                return false;
            }
            //catch (Exception ex)
            //{
            //    Log.Err(ex);
            //    Log.Debug(SQL);
            //    return false;
            //}
        }

        public bool isTableExists(string tablename)
        {
            Int32 count = 0;
            string SQL = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '" + tablename + "'";
            try
            {
                using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(ConnectionString, true))
                {
                    connection.Open();
                    using (System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(SQL, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            if ((reader != null) && (reader.Read())) count = reader.GetInt32(0);
                        }
                    }
                    connection.Close();
                }
                return count > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public string SafeValue(string value)
        {
            if (value == null) return string.Empty;
            return value.Replace('"', '\'');
        }

    }
}