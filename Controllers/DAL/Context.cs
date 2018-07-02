using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Controllers.DAL
{
    public class Context
    {
        SQLiteConnection conn;
        public Context()
        {
            conn = new SQLiteConnection("Data Source=|DataDirectory|NBDB.db;Version=3;");
        }


        public List<string> Account_Status(string account)
        {
            conn.Open();
            List<string> status = new List<string>();
            string sql = "SELECT Victories, Loses FROM Account WHERE Login ='"+account+"'";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();

            status.Add(reader["Victories"].ToString());
            status.Add(reader["Loses"].ToString());

            conn.Close();
            return status;
        }


        public void WL(string account, string result)
        {
            string sql = "";
            conn.Open();

            if (result == "Winner")
                sql = "UPDATE Account SET Victories = (Victories + 1) WHERE Login = '" + account + "'";
            else if (result == "Loser")
                sql = "UPDATE Account SET Loses =(Loses + 1) WHERE Login ='" + account + "'";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public bool registerAccount(string name, string pass)
        {
            conn.Open();
            string sqlSelect = "SELECT Login FROM Account WHERE Login = '" + name + "'";
            SQLiteCommand commandSelect = new SQLiteCommand(sqlSelect, conn);
            SQLiteDataReader reader = commandSelect.ExecuteReader();
            reader.Read();
            if (reader["Login"] == null)
            {
                string sql = "INSERT INTO Account (Login, Password, Victories, Loses) VALUES ('" + name + "', '" + pass + "', 0,0)";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }

            conn.Close();
            return false;
        }

        public bool loginAuthentication(string login, string pass)
        {
            try
            {            
                conn.Open();
                string sql = "SELECT Login, Password FROM Account WHERE Login = '"+ login +"' AND Password = '"+ pass+"'";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();
            
                if (login == reader["Login"].ToString() && pass == reader["Password"].ToString())
                {
                    conn.Close();
                    return true;
                }

            }
            catch
            {
                conn.Close();
                //return false
            }

            return false;
        }

        public string Skill_Type(string char_select, int skill_select)
        {
            conn.Open();
            string sql = "SELECT Type FROM Skills WHERE Name = '" +char_select+ "' AND Skill = "+skill_select;
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();

            string type = reader["Type"].ToString();

            conn.Close();

            return type;
        }

        public List<string> Skills(string skillNumber, string character)
        {
            List<string> chakras = new List<string>();
            try
            {
                conn.Open();
                string sql = "SELECT Taijutsu, Bloodline, Ninjutsu, Genjutsu FROM Skills WHERE Name = '" + character + "' AND Skill = " + skillNumber;

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        chakras.Add(rdr["Taijutsu"].ToString());
                        chakras.Add(rdr["Bloodline"].ToString());
                        chakras.Add(rdr["Ninjutsu"].ToString());
                        chakras.Add(rdr["Genjutsu"].ToString());

                    }
                }

                conn.Close();
                return chakras;
            }
            catch
            {
                conn.Close();
                chakras.Add("0");
                chakras.Add("0");
                chakras.Add("0");
                chakras.Add("0");

                return chakras;
            }
          

        }

        public List<string> return_Characters()
        {
            List<string> chars = new List<string>();
            conn.Open();
    
            string sql = "SELECT Name FROM Character";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        chars.Add(rdr["Name"].ToString());
                    }
                }
            }
            conn.Close();
            return chars;
        }

        public string damage_skill(string skill, string name)
        {
            
            int skillnumber = int.Parse(skill);
            skill = "Skill" + skill;

            conn.Open();
            string sql = "SELECT "+ skill +" FROM Character WHERE Name = '" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();

            string damage = reader[skill].ToString();
            conn.Close();

            return damage;
        }

        public string heal_skill(string skill, string name)
        {

            conn.Open();
            string sql = "SELECT Heal FROM Skills WHERE Name = '" + name + "' AND Skill ="+ skill;
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();

            string heal = reader["Heal"].ToString();
            conn.Close();

            return heal;
        }

    }
}
