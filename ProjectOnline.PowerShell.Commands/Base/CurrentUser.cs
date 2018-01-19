using Microsoft.ProjectServer.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using System.Reflection;

namespace ProjectOnline.PowerShell.Commands.Base
{
    public class PSCurrentUser
    {
        public static User Current { get; set; }
    }
}

