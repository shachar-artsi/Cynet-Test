using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Office_ReportingController : ControllerBase
    {
        // POST func to report entry
        [HttpPost("Report_Entry")]
        public void Reportentry(int employee_id)
        {
            try
            {
                Program.F_Report_Entry(employee_id);
            }
            catch (Exception ex)
            { 
            }
        }
        [HttpGet("List_Entries")]
        // GET func to list entries
        public string List_entries(int employee_id)
        {
            try
            {
               List<string> result = Program.F_List_Entry(employee_id);
               StringBuilder sb = new StringBuilder();

                for (int i = 0; i < result.Count; i++) // Turn to list of dates
                {
                    sb.Append(result[i]);
                    if (i < result.Count - 1)
                    {
                        sb.Append("\n");
                    }
                }

                return sb.ToString();
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost("Report_Covid")]
        public string ReportCovid(string date)
        {
            try
            {
                List<string> result = Program.F_report_covid(DateTime.Parse(date));
                StringBuilder sb = new();

                for (int i = 0; i < result.Count; i++) // Turn to list of dates
                {
                    sb.Append(result[i]);
                    if (i < result.Count - 1)
                    {
                        sb.Append("\n");
                    }
                }

                return sb.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

