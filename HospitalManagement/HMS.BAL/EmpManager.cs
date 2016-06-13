using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HMS.Entity;
using System.Data.Entity;

namespace HMS.BAL
{
   public class EmpManager
    {
        public static List<EmployeeType> GetAll()
       {
           using (HMSTEntities context = new HMSTEntities())
            {
                return context.EmployeeTypes.AsEnumerable().ToList();
            }
       }

    }
}
