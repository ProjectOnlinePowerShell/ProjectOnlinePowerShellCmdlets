using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Management.Automation;
using Microsoft.ProjectServer.Client;
using Microsoft.SharePoint.Client;



namespace ProjectOnline.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PwaEnterpriseCalendars")]
    public class GetEnterpriseCalendars : Cmdlet
    {
        
        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 0, HelpMessage = "Name of the Enterprise resource you wish to get.")]
        public string Name;

        protected override void ProcessRecord()
        {
            CalendarCollection enterpriseCalendars = PSProjectContext.Current.Calendars;

            if (Name != null)
            {
                var calendar = PSProjectContext.Current.LoadQuery(enterpriseCalendars.Where(w => w.Name == Name));
                PSProjectContext.Current.ExecuteQuery();

                WriteObject(calendar.FirstOrDefault());
            }
            else
            {
                var calendars = PSProjectContext.Current.LoadQuery(enterpriseCalendars);

                PSProjectContext.Current.LoadQuery(enterpriseCalendars);
                PSProjectContext.Current.ExecuteQuery();
                WriteObject(calendars);

                Console.WriteLine("All Enterprise Calendars loaded");

            }
        }
    }
}
