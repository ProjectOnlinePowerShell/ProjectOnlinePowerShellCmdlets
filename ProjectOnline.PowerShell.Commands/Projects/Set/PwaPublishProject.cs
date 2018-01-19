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
    [Cmdlet("Publish", "PwaEnterpriseProject")]
    public class PublishEnterpriseProject : Cmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Project object you wish to open.")]
        public PublishedProject Project;

        [Parameter(Mandatory = false, HelpMessage = "Project name you wish to open.")]
        public string Name;

        [Parameter(Mandatory = false, HelpMessage = "Check-in after publish?")]
        public SwitchParameter Checkin;

        protected override void ProcessRecord()
        {
            if (Project != null)
            {
                PSProjectContext.Current.Load(Project);
                PSProjectContext.Current.ExecuteQuery();

                if(Project.IsCheckedOut == true && Project.CheckedOutBy != PSCurrentUser.Current)
                {
                    Console.WriteLine("This project is curently checked out and cannot be published.");
                }
                else if (Project.IsCheckedOut != true)
                {
                    PSProjectContext.Current.Load(Project.CheckOut());
                    PSProjectContext.Current.ExecuteQuery();
                }

                DraftProject draftproj = Project.Draft;                
                PSProjectContext.Current.Load(draftproj);
                PSProjectContext.Current.ExecuteQuery();


                if(Checkin)
                {
                    draftproj.Publish(true);
                    PSProjectContext.Current.ExecuteQuery();

                    WriteObject(draftproj.Publish(true));
                }
                else
                {
                    draftproj.Publish(false);
                    PSProjectContext.Current.ExecuteQuery();

                    WriteObject(draftproj.Publish(false));
                }

            }
            else if (Name != null)
            {

                ProjectCollection enterpriseProjects = PSProjectContext.Current.Projects;

                PublishedProject project = (PublishedProject)PSProjectContext.Current.LoadQuery(enterpriseProjects.Where(w => w.Name == Name));
                PSProjectContext.Current.ExecuteQuery();

                if (project.IsCheckedOut == true && project.CheckedOutBy != PSCurrentUser.Current)
                {
                    Console.WriteLine("This project is curently checked out and cannot be published.");
                }
                else if (project.IsCheckedOut != true)
                {
                    PSProjectContext.Current.Load(project.CheckOut());
                    PSProjectContext.Current.ExecuteQuery();
                }

                DraftProject draftproj = project.Draft;
                PSProjectContext.Current.Load(draftproj);
                PSProjectContext.Current.ExecuteQuery();


                if (Checkin)
                {
                    draftproj.Publish(true);
                    PSProjectContext.Current.ExecuteQuery();
                }
                else
                {
                    draftproj.Publish(false);
                    PSProjectContext.Current.ExecuteQuery();
                }
            }
            else
            {
                Console.WriteLine("No project or name was specified.");

                return;
            }

        }
    }
}