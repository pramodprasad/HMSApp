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
            var specializationlist = new string[] { "Consultant Gen/Laproscopic Surgeon", "General Physician", "Obstetrician & Gynecologist", "Pediatrician", "Consultant Physician", "Ortho", "ENT Surgeon", "Physiotherapist" };
            DoctorName doctorname ;
            List<DoctorName> doctornamelist = new List<DoctorName>();
            using (HMSDBEntities context = new HMSDBEntities())
            {
                
                var doctorlist = context.Doctors.Include("EmployeeDetail").Where(d => specializationlist.Contains(d.Specialization.Name)).AsQueryable();
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

        public static List<DoctorName> GetLabDoctor()
        {
            var specializationlist = new string[] { "Pathologist", "Bio Chemistry" };
            DoctorName doctorname;
            List<DoctorName> doctornamelist = new List<DoctorName>();
            using (HMSDBEntities context = new HMSDBEntities())
            {

                var doctorlist = context.Doctors.Include("EmployeeDetail").Where(d => specializationlist.Contains(d.Specialization.Name)).AsQueryable();
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

        public static List<DoctorName> GetRadiologyDoctor()
        {
            var specializationlist = new string[] { "Radiologist", "Ultrasonically" };
            DoctorName doctorname;
            List<DoctorName> doctornamelist = new List<DoctorName>();
            using (HMSDBEntities context = new HMSDBEntities())
            {

                var doctorlist = context.Doctors.Include("EmployeeDetail").Where(d => specializationlist.Contains(d.Specialization.Name)).AsQueryable();
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

    //public class PataientRegistrationPayment
    //{
    //    public long ID { get; set; }
    //    public string Name { get; set; }
    //    public long ID { get; set; }
    //    public string Name { get; set; }
    //    public long ID { get; set; }
    //    public long ID { get; set; }
    //    public string Name { get; set; }
    //    public long ID { get; set; }
    //    public string Name { get; set; }
    //    public long ID { get; set; }
    //    public string Name { get; set; }
    //    public string Name { get; set; }
    //}
}
