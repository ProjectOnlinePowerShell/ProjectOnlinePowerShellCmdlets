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
    [Cmdlet(VerbsCommon.Get, "PwaContext")]
    public class GetProjContext : Cmdlet
    {

        protected override void ProcessRecord()
        {
            WriteObject(PSProjectContext.Current);
        }
    }
}
