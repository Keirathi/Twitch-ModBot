using System;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace ModBot
{
    class Database
    {
        private SQLiteConnection myDB;
        private SQLiteCommand cmd;
        private string channel;

        public Database()
        {
            channel = ModBot.Properties.Settings.Default.channel;
            InitializeDB();
        }

        private void InitializeDB()
        {
            if (File.Exists("ModBot.sqlite"))
            {
                BackupDatabase();
                myDB = new SQLiteConnection("Data Source=ModBot.sqlite;Version=3;");
                myDB.Open();
                String sql = "CREATE TABLE IF NOT EXISTS '" + channel + "' (id INTEGER PRIMARY KEY, user TEXT, currency INTEGER DEFAULT 0, subscriber INTEGER DEFAULT 0, btag TEXT DEFAULT null, userlevel INTEGER DEFAULT 0);";


                using (var myTransaction = myDB.BeginTransaction())
                {
                    cmd = new SQLiteCommand(sql, myDB);
                    cmd.ExecuteNonQuery();
                    sql = "UPDATE " + channel + " SET user = UPPER(SUBSTR(user, 1, 1)) || SUBSTR(user, 2)";
                    cmd = new SQLiteCommand(sql, myDB);
                    cmd.ExecuteNonQuery();
                    myTransaction.Commit();
                    cmd.Dispose();
                }

                if (tableExists("transfers") && !tableHasData(channel))
                {
                    sql = "INSERT INTO '" + channel + "' SELECT * FROM transfers;";

                    using (cmd = new SQLiteCommand(sql, myDB))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    /*sql = "DROP TABLE transfers;";

                    using (cmd = new SQLiteCommand(sql, myDB))
                    {
                        cmd.ExecuteNonQuery();
                    }*/
                }
                
            }
            else
            {
                SQLiteConnection.CreateFile("ModBot.sqlite");
                myDB = new SQLiteConnection("Data Source=ModBot.sqlite;Version=3;");
                myDB.Open();

                String sql = "CREATE TABLE IF NOT EXISTS '"+channel+"' (id INTEGER PRIMARY KEY, user TEXT, currency INTEGER DEFAULT 0, subscriber INTEGER DEFAULT 0, btag TEXT DEFAULT null, userlevel INTEGER DEFAULT 0);";
                
                using (var myTransaction = myDB.BeginTransaction())
                {
                    cmd = new SQLiteCommand(sql, myDB);
                    cmd.ExecuteNonQuery();
                    sql = "CREATE INDEX 'userName' ON '" + channel + "' ('user' ASC)";
                    cmd = new SQLiteCommand(sql, myDB);
                    cmd.ExecuteNonQuery();
                    sql = "CREATE INDEX 'currencyAMT' on '" + channel + "' ('currency' ASC)";
                    cmd = new SQLiteCommand(sql, myDB);
                    cmd.ExecuteNonQuery();
                    myTransaction.Commit();
                }
            }
        }

        public void newUser(String user)
        {
            if (!userExists(user))
            {
                String sql = "INSERT INTO '"+channel+"' (user) VALUES ('" + user + "');";
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void newUser(String[] users)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var myTransaction = myDB.BeginTransaction())
            {
                using (cmd = myDB.CreateCommand())
                {
                    //cmd.Transaction = myTransaction;
                    cmd.CommandText = "INSERT INTO '" + channel + "' (user, currency) VALUES (@user, 1)";
                    cmd.Parameters.AddWithValue("@user", "");
                    foreach (string person in users)
                    {
                        cmd.Parameters["@user"].Value = person;
                        cmd.ExecuteNonQuery();
                    }
                }
                myTransaction.Commit();
                sw.Stop();
                //Console.WriteLine("Adding all new users took: " + sw.Elapsed);
            }
        }

        public int checkCurrency(String user)
        {
            if (userExists(user)) {
                String sql = "SELECT * FROM '" + channel + "' WHERE user = \"" + user + "\";";
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            //Console.WriteLine("1: " + r["currency"].ToString());
                            return int.Parse(r["currency"].ToString());
                        }
                        else return 0;
                    }
                }
            }
            else {
                newUser(user);
                return 0;
            }
        }

        public bool addCurrency(String user, int amount)
        {
            if (userExists(user))
            {
                using (cmd = myDB.CreateCommand())
                {
                    cmd.CommandText = "UPDATE '" + channel + "' SET currency = currency + @amount WHERE user = @user";
                    cmd.Parameters.Add(new SQLiteParameter("@amount", amount));
                    cmd.Parameters.Add(new SQLiteParameter("@user", user));
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            else return false;
        }

        public void addCurrency(String[] users, int amount)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool space = false;
            StringBuilder sb = new StringBuilder();
            using (var myTransaction = myDB.BeginTransaction())
            {
                using (cmd = myDB.CreateCommand())
                {
                    //cmd.Transaction = myTransaction;
                    cmd.CommandText = "UPDATE '" + channel + "' SET currency = currency + @amount WHERE user = @user";
                    cmd.Parameters.AddWithValue("@user", "");
                    cmd.Parameters.AddWithValue("@amount", amount);
                    foreach (string person in users)
                    {
                        if (!userExists(person))
                        {
                            if (space)
                            {
                                sb.Append(" ");
                            }
                            sb.Append(person);
                            space = true;
                        }
                        else
                        {
                            cmd.Parameters["@user"].Value = person;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                myTransaction.Commit();
                sw.Stop();
                //Console.WriteLine("Updating users took: " + sw.Elapsed);
                newUser(sb.ToString().Split(' '));
            }
        }

        public bool removeCurrency(String user, int amount)
        {
            if (userExists(user))
            {
                if (amount > checkCurrency(user))
                {
                    amount = checkCurrency(user);
                }
                using (cmd = myDB.CreateCommand())
                {
                    cmd.CommandText = "UPDATE '" + channel + "' SET currency = currency - @amount WHERE user = @user";
                    cmd.Parameters.Add(new SQLiteParameter("@amount", amount));
                    cmd.Parameters.Add(new SQLiteParameter("@user", user));
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            else return false;
        }

        public bool userExists(String user)
        {
            String sql = "SELECT * FROM '" + channel + "' WHERE user = '" + user + "';";
            using (var tcmd = new SQLiteCommand(sql, myDB))
            {
                using (SQLiteDataReader r = tcmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        if (r["user"].ToString().Equals(user, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public String getBtag(String user)
        {
            if (userExists(user))
            {
                String sql = "SELECT * FROM '" + channel + "' WHERE user = \"" + user + "\";";
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            //Console.WriteLine(r["btag"]);
                            if (System.DBNull.Value.Equals(r["btag"]))
                            {
                                //Console.WriteLine("btag is null");
                                return null;
                            }
                            else return r["btag"].ToString();
                        }
                        else return null;
                    }
                }
            }
            else {
                newUser(user);
                return null;
            }
        }

        public void setBtag(String user, String btag)
        {
            if (!userExists(user))
            {
                newUser(user);
            }
            using (cmd = myDB.CreateCommand())
            {
                cmd.CommandText = "UPDATE '" + channel + "' SET btag = @btag WHERE user = @user";
                cmd.Parameters.Add(new SQLiteParameter("@btag", btag));
                cmd.Parameters.Add(new SQLiteParameter("@user", user));
                Console.WriteLine(cmd.CommandText);
                cmd.ExecuteNonQuery();
            }
        }

        public bool isSubscriber(String user)
        {
            if (!userExists(user))
            {
                newUser(user);
                return false;
            }
            else
            {
                String sql = "SELECT * FROM '" + channel + "' WHERE user = \"" + user + "\";";
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            if (int.Parse(r["subscriber"].ToString()) == 1)
                            {
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                }
            }
        }

        public bool addSub(String user)
        {
            if (userExists(user))
            {
                String sql = String.Format("UPDATE '" + channel + "' SET subscriber = 1 WHERE user = \"{0}\";", user);
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            return false;
        }

        public bool removeSub(String user)
        {
            if (userExists(user))
            {
                String sql = String.Format("UPDATE '" + channel + "' SET subscriber = 0 WHERE user = \"{0}\";", user);
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            return false;
        }

        public int getUserLevel(String user)
        {
            if (!userExists(user))
            {
                newUser(user);
                return 0;
            }
            else
            {
                String sql = "SELECT * FROM '" + channel + "' WHERE user = \"" + user + "\";";
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            int level;
                            if (int.TryParse(r["userlevel"].ToString(), out level))
                            {
                                return level;
                            }
                            else return 0;
                        }
                        else return 0;
                    }
                }
            }
        }

        public void setUserLevel(String user, int level)
        {

            String sql = "UPDATE '" + channel + "' SET userlevel = \"" + level + "\" WHERE user = \"" + user + "\";";
            using (cmd = new SQLiteCommand(sql, myDB))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private bool tableExists(String table)
        {
            String sql = "SELECT COUNT(*) FROM sqlite_master WHERE name = '" + table + "';";
            try
            {
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            if (int.Parse(r["COUNT(*)"].ToString()) != 0)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public int totalUsers()
        {
            int count = 0;
            string sql = "SELECT user FROM '" + channel + "';";
            try
            {
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            count++;
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return count;
        }

        public string[] getTop(int count)
        {
            List<string> users = new List<string>();
            string sql = "SELECT user from '" + channel + "' ORDER BY currency DESC;";
            try
            {
                using (cmd = new SQLiteCommand(sql, myDB))
                {
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (r.Read())
                            {
                                users.Add(r["user"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return users.ToArray();
        }

        private bool tableHasData(String table)
        {
            String sql = "SELECT * FROM '" + table + "';";

            using (cmd = new SQLiteCommand(sql, myDB))
            {
                using (SQLiteDataReader r = cmd.ExecuteReader())
                {
                    if (r.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private void BackupDatabase()
        {
            string date = DateTime.Today.ToString("d").Replace('/', '-');
            string time = DateTime.Now.ToString("HH-mm");

            string sourcePath = Directory.GetCurrentDirectory();
            string targetPath = Path.Combine(sourcePath, "Database Backups");

            string sourceFile = Path.Combine(sourcePath, "ModBot.sqlite");
            string targetFile = Path.Combine(targetPath, date + "_" + time + "_ModBot.sqlite");

            //Console.WriteLine(targetFile);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            
            File.Copy(sourceFile, targetFile, true);
        }

        private void FixDB()
        {

        }

        private String capName(String user)
        {
            return char.ToUpper(user[0]) + user.Substring(1);
        }
    }
}