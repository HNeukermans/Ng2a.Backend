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
}
