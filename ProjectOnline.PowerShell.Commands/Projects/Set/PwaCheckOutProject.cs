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
    [Cmdlet("CheckOut", "PwaEnterpriseProjects")]
    public class CheckOutEnterpriseProject : Cmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Project object you wish to open.")]
        public PublishedProject Project;

        protected override void ProcessRecord()
        {
            if (Project != null)
            {
                PSProjectContext.Current.Load(Project.CheckOut());
                PSProjectContext.Current.ExecuteQuery();
                WriteObject(Project.CheckOut());
            }
        }
    }
}