using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controllers.DAL;

namespace Controllers
{
    public class Tools
    {
        public int Convert_Name_Int(string name)
        {
            name = String.Join("", System.Text.RegularExpressions.Regex.Split(name, @"[^\d]"));
            return int.Parse(name);
        }

        public int Conversion_Object_Toint(object obj)
        {
            int obj_int = int.Parse(obj.ToString());
            return obj_int;
        }
    }
}
