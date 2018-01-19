﻿using System;
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
    [Cmdlet(VerbsCommon.Set, "PwaEnterpriseResourceAsActive")]
    public class ActivateEnterpriseResource : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = false, Position = 0, HelpMessage = "Enterprise resource object you wish to activate.")]
        public EnterpriseResource Resource;

        protected override void ProcessRecord()
        {
            Resource.IsActive = true;
            PSProjectContext.Current.EnterpriseResources.Update();
            PSProjectContext.Current.ExecuteQuery();
        }
    }
}