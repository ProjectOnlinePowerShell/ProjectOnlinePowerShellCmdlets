using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Management.Automation;
using Microsoft.ProjectServer.Client;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint;

namespace ProjectOnline.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Set, "PwaEnterpriseResource")]
    public class SetEnterpriseResource : Cmdlet
    {
            [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 0, HelpMessage = "URL of the Project Web App site you wish to connect to.")]
            public EnterpriseResource EnterpriseResource;

            [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 1, HelpMessage = "URL of the Project Web App site you wish to connect to.")]
            public User User;

            [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 2, HelpMessage = "URL of the Project Web App site you wish to connect to.")]
            public string Name;

        //  Fields to update
            [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 3, HelpMessage = "URL of the Project Web App site you wish to connect to.")]
            public string Email;

            [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 4, HelpMessage = "URL of the Project Web App site you wish to connect to.")]
            public DateTime EarliestAvailable;

            [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 5, HelpMessage = "URL of the Project Web App site you wish to connect to.")]
            public DateTime LatestAvailable;

            [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 6, HelpMessage = "URL of the Project Web App site you wish to connect to.")]
            public bool Active;


            protected override void ProcessRecord()
            {
                EnterpriseResourceCollection enterpriseResources = PSProjectContext.Current.EnterpriseResources;

            //  Get Enterprise Resource
                if (User != null)
                {
                    EnterpriseResource EnterpriseResource = (EnterpriseResource)PSProjectContext.Current.LoadQuery(enterpriseResources.Where(w => w.User == User));
                    PSProjectContext.Current.ExecuteQuery();
                }
                else if (Name != null)
                {
                    EnterpriseResource EnterpriseResource = (EnterpriseResource)PSProjectContext.Current.LoadQuery(enterpriseResources.Where(w => w.Name == Name));
                    PSProjectContext.Current.ExecuteQuery();
                }


            //  Update Enterprise Resource
                if (Email != null)
                {
                    EnterpriseResource.Email = Email;
                }

                if (EarliestAvailable != null)
                {
                    EnterpriseResource.HireDate = EarliestAvailable.Date;
                }

                if (LatestAvailable != null)
                {
                    EnterpriseResource.TerminationDate = LatestAvailable.Date;
                }

            //  Update Resource
                PSProjectContext.Current.EnterpriseResources.Update();
                PSProjectContext.Current.ExecuteQuery();

                if (!Active)
                {
                    if (Active == true && EnterpriseResource.IsActive == false)
                    {
                        EnterpriseResource.IsActive = true;
                        PSProjectContext.Current.EnterpriseResources.Update();
                        PSProjectContext.Current.ExecuteQuery();
                    }
                    else if (Active == false && EnterpriseResource.IsActive == true)
                    {
                        EnterpriseResource.IsActive = false;
                        PSProjectContext.Current.EnterpriseResources.Update();
                        PSProjectContext.Current.ExecuteQuery();
                    }
                }
            }
        }
    }

