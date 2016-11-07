using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ng2Aa_demo.Domain.Avatar
{
    public class ActiveDirectorySettings
    {
        public static ActiveDirectorySettings Production;

        static ActiveDirectorySettings() {
            Production = new ActiveDirectorySettings();
            Production.AuthString = "https://login.microsoftonline.com/hneu70532.onmicrosoft.com";
            Production.ClientId = "1be0ece5-3285-459e-bacc-18ecc5104691";
            Production.ClientSecret = "tLSq8YmbFAlmz7OnHpWOo/hFqXeT/JmJu08u07fqcQs=";
            Production.ResourceUrl = "https://graph.windows.net";
            Production.TenantId = "70904889-0180-4beb-ab8e-0d884f481f23";            
        }


        public string TenantId { get; set; }
        public string ResourceUrl { get; set; }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
        public string AuthString { get; set; }
    }
}
