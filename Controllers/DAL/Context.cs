using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Controllers.DAL
{
    class Context
    {
        SQLiteConnection conn;
        public Context()
        {
            conn = new SQLiteConnection("Data Source=;Version=3;");
        }
    }
}
