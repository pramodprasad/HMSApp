using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Entity;

namespace HMS.BAL
{
    public class UtilityManager
    {
        public static List<DoctorName> GetName()
        {
            DoctorName doctorname ;
            List<DoctorName> doctornamelist = new List<DoctorName>();
            using (HMSTEntities context = new HMSTEntities())
            {
                
                var doctorlist = context.Doctors.Include("EmployeeDetail").AsQueryable();
                foreach (var item in doctorlist)
                {
                    doctorname = new DoctorName();
                    doctorname.ID = item.ID;
                    doctorname.Name = item.EmployeeDetail.FirstName + " " + item.EmployeeDetail.LastName;
                    doctornamelist.Add(doctorname);
                }
               
            }
            return doctornamelist;
        }
    }

    public class DoctorName
    {
        public long ID {get;set;}
        public string Name { get; set; }
    }
}
