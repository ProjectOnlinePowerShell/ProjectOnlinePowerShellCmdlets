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
    [Cmdlet(VerbsCommon.Get, "PwaDraftProject")]
    public class GetDraftProject : Cmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Project object you wish to open.")]
        public PublishedProject Project;

        protected override void ProcessRecord()
        {
            PSProjectContext.Current.Load(Project.Draft);
            PSProjectContext.Current.ExecuteQuery();
            WriteObject(Project.Draft);
        }
    }
}