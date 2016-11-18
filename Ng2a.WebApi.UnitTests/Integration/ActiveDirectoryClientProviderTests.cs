using Ng2Aa_demo.Domain.Avatar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace ImplementWebJobs.UnitTests.WebApi.Domain.Avatar
{
    public class ActiveDirectoryClientProviderTests
    {
        [Fact]
        public void Get_Should_Return_Client()
        {
            var settings = ActiveDirectory.GetProductionSettings();
            var client = new ActiveDirectoryClientProvider(settings).Get();
        }
    }
}
