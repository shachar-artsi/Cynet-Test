using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace WebApplication3
{
    public class Program
    {
        public static string cs = @"server=cynetmysql.cggu20qir3eq.eu-central-1.rds.amazonaws.com;userid=admin;password=Aa123456!;database=CynetMySQL";
       // public static List<Office_Reporting.EntryLog> Entrylog = new(); // Perishable entry log, For Parishable Log
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void F_Report_Entry (int id)
        {

            //Entrylog.Add(new Office_Reporting.EntryLog(id, DateTime.Now)); // For Parishable Log

            try
            {
                using var con = new MySqlConnection(cs);
                con.Open();
                string sql = "INSERT INTO Entry_Log (`employee_id`, `date`) VALUES (" + id.ToString() + ", \"" + DateTime.Now.ToShortDateString() + "\");";
                using var cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }

        }
        public static List<string> F_List_Entry(int id) // List entries of single employee
        {
            /*List<string> result = new(); // For Parishable Log
            try
            {
                foreach (Office_Reporting.EntryLog entryLog in Entrylog)
                {
                    if (entryLog.ID == id)
                        result.Add(entryLog.Time.ToString());
                }
                if (result.Count > 0)
                    return result;
                else
                    return null;
            }
            catch (Exception ex) { return null; }*/

            try
            {
                using var con = new MySqlConnection(cs);
                con.Open();

                List<string> res = new();

                string sql = "SELECT date FROM Entry_Log WHERE employee_id = " + id.ToString();
                using var cmd = new MySqlCommand(sql, con);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    res.Add(rdr.GetValue(0).ToString());
                }

                return res;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static List<string> F_report_covid(DateTime date) // List employees who were in office on same day as infected
        {
            List<int> result = new();

            using var con = new MySqlConnection(cs);
            con.Open();

  

            string sql = "SELECT employee_id, date FROM Entry_Log";
            using var cmd = new MySqlCommand(sql, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Office_Reporting.EntryLog entryLog = new(int.Parse(rdr.GetValue(0).ToString()), DateTime.Parse(rdr.GetValue(1).ToString()));

                TimeSpan entryepoch = (entryLog.Time.Date - new DateTime(1970, 1, 1));
                TimeSpan infectepoch = (date - new DateTime(1970, 1, 1));

                if ((infectepoch.TotalSeconds <= entryepoch.TotalSeconds) && (infectepoch.TotalSeconds >= entryepoch.TotalSeconds - 604800))      //((entryLog.Time.Date <= date.Date) && (entryLog.Time.Date >= date.Date.Subtract(DateTime) 
                {
                    result.Add(entryLog.ID);
                }
            }
            return Send_alert(result);
        }

        private static List<string> Send_alert(List<int> employeelist) // Generate a list of emails to alert
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            List<string> res = new();

            string sql = "SELECT email, id FROM Employee_List";
            using var cmd = new MySqlCommand(sql, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                if (employeelist.Contains((Int32)rdr.GetValue(1)))
                    res.Add(rdr.GetValue(0).ToString());
            }

            return res;
            
        }
    }

}
