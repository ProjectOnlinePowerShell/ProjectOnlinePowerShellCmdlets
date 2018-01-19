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
    [Cmdlet(VerbsCommon.Get, "PwaEnterpriseProjects")]
    public class GetEnterpriseProject : Cmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Project oject that you wish to open.")]
        public PublishedProject Project;
        [Parameter(Mandatory = false, HelpMessage = "Name of the project you wish to open.")]
        public string Name;

        protected override void ProcessRecord()
        {

            if (Project != null)
            {
                PSProjectContext.Current.Load(Project);
                PSProjectContext.Current.ExecuteQuery();
                WriteObject(Project);

            }
            else if (Name != null)
            {
                ProjectCollection enterpriseProjects = PSProjectContext.Current.Projects;

                var project = PSProjectContext.Current.LoadQuery(enterpriseProjects.Where(w => w.Name == Name));
                PSProjectContext.Current.ExecuteQuery();

                WriteObject(project.FirstOrDefault());
            }
            else
            {
                ProjectCollection enterpriseProjects = PSProjectContext.Current.Projects;

                var projects = PSProjectContext.Current.LoadQuery(enterpriseProjects);

                PSProjectContext.Current.Load(enterpriseProjects);
                PSProjectContext.Current.ExecuteQuery();
                WriteObject(projects);
            }
        }

    }
}