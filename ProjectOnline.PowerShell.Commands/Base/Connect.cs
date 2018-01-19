using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Management.Automation;
using Microsoft.ProjectServer.Client;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.DocumentManagement;
//using System.Management.Automation.Host.PSHost;


namespace ProjectOnline.PowerShell.Commands.Base
{
    [Cmdlet("Connect","PWAOnline")]
    public class ConnectPWAOnline : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "URL of the Project Web App site you wish to connect to.")]
        public string Url;

        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The username of the account you wish to connect to the PWA site with.  This account should be both a Site Collection and a Project Web App Administrator.")]
        public string Username;

        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The password of the account you wish to connect to the PWA site with.")]
        public string Password;

        [Parameter(Mandatory = false, HelpMessage = "Credentials of the user to connect with. Either specify a PSCredential object or a string. In case of a string value a lookup will be done to the Windows Credential Manager for the correct credentials.")]
        public string Credentials;

        [Parameter(Mandatory = false, HelpMessage = "Credentials of the user to connect with. Either specify a PSCredential object or a string. In case of a string value a lookup will be done to the Windows Credential Manager for the correct credentials.")]
        public bool UseWebLogin;

        protected override void ProcessRecord()
        {
            PSCredential creds = null;
            if (Credentials != null)
            {
                creds = GetCredentials();
                Username = creds.UserName;

                PSProjectContext.Current = new ProjectContext(Url);
                PSProjectContext.Current.Credentials = new SharePointOnlineCredentials(creds.UserName, creds.Password);

                Site site = PSProjectContext.Current.Site;
                PSProjectContext.Current.Load(site);
                PSProjectContext.Current.ExecuteQuery();
            }
            else if (UseWebLogin)
            {
                //PSProjectContext.Current = ConnectionHelper.InstantiateWebloginConnection(new Uri(Url));
            }
            else
            {
                var securePassword = new SecureString();
                foreach (char c in Password) securePassword.AppendChar(c);
            

                PSProjectContext.Current = new ProjectContext(Url);
                PSProjectContext.Current.Credentials = new SharePointOnlineCredentials(Username, securePassword);

                Site site = PSProjectContext.Current.Site;
                PSProjectContext.Current.Load(site);
                PSProjectContext.Current.ExecuteQuery();
            }

            EnterpriseResourceCollection enterpriseResources = PSProjectContext.Current.EnterpriseResources;

            var resource = PSProjectContext.Current.LoadQuery(enterpriseResources.Where(w => w.Email == Username));
            PSProjectContext.Current.ExecuteQuery();

            PSCurrentUser.Current = resource.FirstOrDefault().User;




        }

        private PSCredential GetCredentials()
        {
            PSCredential creds;

            var connectionUri = new Uri(Url);

            // Try to get the credentials by full url

            creds = SharePointPnP.PowerShell.Commands.Utilities.CredentialManager.GetCredential(Credentials);
            if (creds == null)
            {
                // Try to get the credentials by splitting up the path
                //var pathString = $"{connectionUri.Scheme}://{(connectionUri.IsDefaultPort ? connectionUri.Host : $"{connectionUri.Host}:{connectionUri.Port}")}";
                var path = connectionUri.AbsolutePath;
                while (path.IndexOf('/') != -1)
                {
                    path = path.Substring(0, path.LastIndexOf('/'));
                    if (!string.IsNullOrEmpty(path))
                    {
                        //var pathUrl = $"{pathString}{path}";
                        //creds = SharePointPnP.PowerShell.Commands.Utilities.CredentialManager.GetCredential(pathUrl);
                        if (creds != null)
                        {
                            break;
                        }
                    }
                }

                if (creds == null)
                {
                    // Try to find the credentials by schema and hostname
                    creds = SharePointPnP.PowerShell.Commands.Utilities.CredentialManager.GetCredential(connectionUri.Scheme + "://" + connectionUri.Host);

                    if (creds == null)
                    {
                        // try to find the credentials by hostname
                        creds = SharePointPnP.PowerShell.Commands.Utilities.CredentialManager.GetCredential(connectionUri.Host);
                    }
                }

            }

            return creds;
        }
    }



}
