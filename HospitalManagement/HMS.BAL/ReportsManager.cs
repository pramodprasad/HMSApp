using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Entity;
using System.Data;
using System.Reflection;

namespace HMS.BAL
{
   public  class ReportsManager
    {
       public static DataTable GetBranchDetails( long id)
       {
           DataTable dt = new DataTable();
           using (HMSDBEntities context = new HMSDBEntities())
           {
               var branchdetails = context.sp_BranchDetails(id);
               if (branchdetails != null)
               {
                   dt = ExtensionMethods.ConvertToDataTable(branchdetails);
               }
             


               //PropertyInfo[] branchDetailColumns = branchdetails.GetType().GetProperties();
               //DataTable dt = new DataTable();
               //foreach (var col in branchDetailColumns)
               //{
               //    DataColumn column = new DataColumn();
               //    column.Caption = col.Name;
               //    column.DataType = col.PropertyType;

               //    dt.Columns.Add(column);
               //}

               //foreach (var record in branchDetailColumns)
               //{
               //    DataRow row = dt.NewRow();
               //    for (int i = 0; i < dt.Columns.Count; i++)
               //        row[i] = branchDetailColumns[i].GetValue(record);
               //}
               return dt;
           }
       }
      

    }

   public static class ExtensionMethods
   {
       public static DataTable AsDataTable<T>(this IEnumerable<T> list)
       {
           DataTable dtOutput = new DataTable("tblOutput");

           //if the list is empty, return empty data table
           //if (list.Count() == 0)
           //    return dtOutput;

           //get the list of  public properties and add them as columns to the
           //output table    
           PropertyInfo[] properties = list.FirstOrDefault().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
          // PropertyInfo[] properties = list.FirstOrDefault().GetType().GetProperties(BindingFlags.Public);
           foreach (PropertyInfo propertyInfo in properties)
           {
               dtOutput.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
           }
               

           //populate rows
           DataRow dr;
           //iterate through all the objects in the list and add them
           //as rows to the table
           foreach (T t in list)
           {
               dr = dtOutput.NewRow();
               //iterate through all the properties of the current object
               //and set their values to data row
               foreach (PropertyInfo propertyInfo in properties)
               {
                   dr[propertyInfo.Name] = propertyInfo.GetValue(t, null);
               }
               dtOutput.Rows.Add(dr);
           }
           return dtOutput;
       }

       public static DataTable ConvertToDataTable<T>(IEnumerable<T> varlist)
       {
           using (DataTable dtReturn = new DataTable())
           {

               // column names  
               PropertyInfo[] oProps = null;

               if (varlist == null) return dtReturn;

               foreach (T rec in varlist)
               {
                   // Use reflection to get property names, to create table, Only first time, others will follow  
                   if (oProps == null)
                   {
                       oProps = ((Type)rec.GetType()).GetProperties();
                       foreach (PropertyInfo pi in oProps)
                       {
                           Type colType = pi.PropertyType;

                           if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                           {
                               colType = colType.GetGenericArguments()[0];
                           }

                           dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                       }
                   }

                   DataRow dr = dtReturn.NewRow();

                   foreach (PropertyInfo pi in oProps)
                   {
                       dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                       (rec, null);
                   }

                   dtReturn.Rows.Add(dr);
               }
               return dtReturn;
           }
       }
   }
}
