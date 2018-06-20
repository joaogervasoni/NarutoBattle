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

        //public List<string> returnSkill_Chakra(string character)
        //{
        //    List<string> skillChakra = new List<string>();
        //    conn.Open();
        //    string sql = "SELECT Taijutsu, Bloodline, Ninjutsu, Genjutsu FROM Skills WHERE Name = '" + character + "' AND Skill = " + skillNumber;
        //}

        public List<string> Skills(string skillNumber, string character)
        {

            List<string> chakras = new List<string>();
            conn.Open();
            string sql = "SELECT Taijutsu, Bloodline, Ninjutsu, Genjutsu FROM Skills WHERE Name = '"+ character+ "' AND Skill = "+skillNumber;

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
            //List<string> Skills = new List<string>();
            //List<string> Taijutsu = new List<string>();
            //List<string> Bloodline = new List<string>();
            //List<string> Ninjutsu = new List<string>();
            //List<string> Genjutsu = new List<string>();
            //conn.Open();
            //string sql = "SELECT Skill, Taijutsu, Bloodline, Ninjutsu, Genjutsu FROM Skills WHERE Name = Sarada";

            //using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            //{
            //    using (SQLiteDataReader rdr = cmd.ExecuteReader())
            //    {
            //        while (rdr.Read())
            //        {
            //            Skills.Add(rdr["Skill"].ToString());
            //            Taijutsu.Add(rdr["Taijutsu"].ToString());
            //            Bloodline.Add(rdr["Bloodline"].ToString());
            //            Ninjutsu.Add(rdr["Ninjutsu"].ToString());
            //            Genjutsu.Add(rdr["Genjutsu"].ToString());
            //        }
            //    }
            //}

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

        public object teste()
        {
            conn.Open();
            string sql = "SELECT Name FROM Character";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();
            //dbConnection.Close();

            return reader["Name"];
        }

    }
}
