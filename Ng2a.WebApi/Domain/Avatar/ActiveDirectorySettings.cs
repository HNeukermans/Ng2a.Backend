using Ng2Aa_demo.Domain.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ng2Aa_demo.Domain.Avatar
{
    public class ActiveDirectorySettings
    {
        

        public string TenantId { get; set; }
        public string ResourceUrl { get; set; }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
        public string AuthString { get; set; }
    }

    public class ActiveDirectory
    {
        public static ActiveDirectorySettings GetProductionSettings()
        {
            var s = new ActiveDirectorySettings();
            s.AuthString = "https://login.microsoftonline.com/hneu70532.onmicrosoft.com";
            s.ClientId = "1be0ece5-3285-459e-bacc-18ecc5104691";
            s.ClientSecret = "tLSq8YmbFAlmz7OnHpWOo/hFqXeT/JmJu08u07fqcQs=";
            s.ResourceUrl = "https://graph.windows.net";
            s.TenantId = "70904889-0180-4beb-ab8e-0d884f481f23";
            return s;
        }
    }
}

    
