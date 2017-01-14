using FluentAssertions;
using Ng2Aa_demo.Domain.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ng2a.WebApi.UnitTests.Integration
{
    public class InMemoryAvatarCacheTests
    {
        [Fact]
        public async Task Load_Should_Fetch_Data()
        {
            var settings = ActiveDirectory.GetProductionSettings();
            var client = new ActiveDirectoryClientProvider(settings).Get();
            var cache = new InMemoryAvatarCache(client);

            var avatars = await cache.Get();

            avatars.Count.Should().BeGreaterThan(0);
        }
    }
}
