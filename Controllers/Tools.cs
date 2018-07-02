using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controllers.DAL;

namespace Controllers
{
    public class Tools : Context
    {
        public int convert_name_int(string name)
        {

            name = String.Join("", System.Text.RegularExpressions.Regex.Split(name, @"[^\d]"));
            return int.Parse(name);
        }

        public int conversion_object_toint(object obj)
        {
            int obj_int = int.Parse(obj.ToString());
            return obj_int;
        }


    }
}
