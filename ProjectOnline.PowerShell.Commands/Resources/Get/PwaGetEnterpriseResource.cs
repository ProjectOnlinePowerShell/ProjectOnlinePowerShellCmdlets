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
    [Cmdlet(VerbsCommon.Get, "PwaEnterpriseResources")]
    public class GetEnterpriseResource : Cmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 0, HelpMessage = "SharePoint.Client.User object of the Enterprise Resource to get.")]
        public User User;
        
        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 0, HelpMessage = "Name of the Enterprise Resource to get.")]
        public string Name;

        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 0, HelpMessage = "Email of the Enterprise Resource to get.")]
        public string Email;

        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 0, HelpMessage = "Exclude generic resources.")]
        public SwitchParameter ExcludeGenerics;

        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 0, HelpMessage = "Only return users.")]
        public SwitchParameter UsersOnly;

        protected override void ProcessRecord()
        {
            EnterpriseResourceCollection enterpriseResources = PSProjectContext.Current.EnterpriseResources;

            if (User != null)
            {
                var resource = PSProjectContext.Current.LoadQuery(enterpriseResources.Where(w => w.User == User));
                PSProjectContext.Current.ExecuteQuery();

                WriteObject(resource.FirstOrDefault());

            }
            else if (Name != null)
            {
                var resource = PSProjectContext.Current.LoadQuery(enterpriseResources.Where(w => w.Name == Name));
                PSProjectContext.Current.ExecuteQuery();

                WriteObject(resource.FirstOrDefault());

            }
            else if (Email != null)
            {
                var resource = PSProjectContext.Current.LoadQuery(enterpriseResources.Where(w => w.Email == Email));
                PSProjectContext.Current.ExecuteQuery();

                WriteObject(resource.FirstOrDefault());

            }
            else if (ExcludeGenerics)
            {
                var resources = PSProjectContext.Current.LoadQuery(enterpriseResources.Where(w => w.IsGeneric == false));
                PSProjectContext.Current.LoadQuery(enterpriseResources);
                PSProjectContext.Current.ExecuteQuery();
                WriteObject(resources);
            }
            else if (UsersOnly)
            {
                var resources = PSProjectContext.Current.LoadQuery(enterpriseResources.Where(w => w.IsGeneric == false));
                PSProjectContext.Current.ExecuteQuery();
                Console.WriteLine("Processing... this may take a while to complete!");
                List<EnterpriseResource> list = new List<EnterpriseResource>();
                foreach (EnterpriseResource resource in resources)
                {
                    var user = resource.User;
                    PSProjectContext.Current.Load(user);
                    PSProjectContext.Current.ExecuteQuery();

                    if(user.ServerObjectIsNull == false)
                    {
                        list.Add(resource);
                    }
                    else
                    {
                    }
                }
                PSProjectContext.Current.LoadQuery(enterpriseResources.Where(w => list.Contains(w)));
                PSProjectContext.Current.ExecuteQuery();
                WriteObject(enterpriseResources);
            }
            else
            {
                var resources = PSProjectContext.Current.LoadQuery(enterpriseResources);
                PSProjectContext.Current.LoadQuery(enterpriseResources);
                PSProjectContext.Current.ExecuteQuery();
                WriteObject(resources);
            }
        }
    }
}