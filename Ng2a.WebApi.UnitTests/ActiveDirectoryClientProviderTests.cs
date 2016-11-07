using Ng2Aa_demo.Domain.Avatar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplementWebJobs.UnitTests.WebApi.Domain.Avatar
{
    //[TestClass]
    public class ActiveDirectoryClientProviderTests
    {
        [Fact]
        public async Task Get_Should_Return_Client()
        {
            var settings = new ActiveDirectorySettings();
            var client = new ActiveDirectoryClientProvider(settings).Get();
        }
    }
}
