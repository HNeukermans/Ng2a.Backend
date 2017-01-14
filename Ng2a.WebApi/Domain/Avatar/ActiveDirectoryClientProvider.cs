using Microsoft.Azure.ActiveDirectory.GraphClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Ng2Aa_demo.Domain.Avatar
{
    public class ActiveDirectoryClientProvider
    {
        public static string TokenForApplication;
        public ActiveDirectorySettings _settings;

        public ActiveDirectoryClientProvider(ActiveDirectorySettings settings) {
            _settings = settings;
        }

        public ActiveDirectoryClient Get()
        {
            Uri servicePointUri = new Uri(_settings.ResourceUrl);
            Uri serviceRoot = new Uri(servicePointUri, _settings.TenantId);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                async () => await AcquireTokenAsyncForApplication());
            return activeDirectoryClient;
        }

        private async Task<string> AcquireTokenAsyncForApplication()
        {
            return await GetTokenForApplication();
        }

        private async Task<string> GetTokenForApplication()
        {
            if (TokenForApplication == null)
            {
                AuthenticationContext authenticationContext = new AuthenticationContext(_settings.AuthString, false);
                // Config for OAuth client credentials 
                ClientCredential clientCred = new ClientCredential(_settings.ClientId,
                    _settings.ClientSecret);
                AuthenticationResult authenticationResult =
                    await authenticationContext.AcquireTokenAsync(_settings.ResourceUrl,
                        clientCred);
                TokenForApplication = authenticationResult.AccessToken;
            }
            return TokenForApplication;
        }
    }

   
}
