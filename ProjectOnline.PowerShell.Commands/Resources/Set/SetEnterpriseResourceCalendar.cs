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
    [Cmdlet(VerbsCommon.Set, "PwaEnterpriseResourceCalendar")]
    public class SetEnterpriseCalendars : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = false, Position = 0, HelpMessage = "Name of the Enterprise resource you wish to set.")]
        public EnterpriseResource Resource;

        [Parameter(Mandatory = true, ValueFromPipeline = false, Position = 0, HelpMessage = "Name of the Enterprise resource you wish to set.")]
        public Calendar Calendar;

        protected override void ProcessRecord()
        {
            Resource.BaseCalendar = Calendar;
            PSProjectContext.Current.EnterpriseResources.Update();
            PSProjectContext.Current.ExecuteQuery();

            Console.WriteLine("Calendar updated for " + Resource.Name + " to " + Calendar.Name);
        }
    }
}
