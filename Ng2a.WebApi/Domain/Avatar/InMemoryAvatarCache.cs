using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ng2Aa_demo.Domain.Avatar
{
    public class InMemoryAvatarCache
    {
        private readonly ActiveDirectoryClientProvider _adClientProvider;
        private List<Tuple<string, Stream>> _cache;

        public InMemoryAvatarCache(ActiveDirectoryClientProvider adClientProvider)
        {
            _adClientProvider = adClientProvider;
            _cache = null;
        }

        public async Task<List<Tuple<string, Stream>>> Load() {
            if (_cache != null) return _cache;
            
            _cache = new List<Tuple<string, Stream>>();

            var client = _adClientProvider.Get();

            List<IUser> usersList = null;
            IPagedCollection<IUser> searchResults = null;

            IPagedCollection<IUser> userCollection = await client.Users.ExecuteAsync();

            do
            {
                var users = userCollection.CurrentPage.ToList();
                foreach (IUser user in users)
                {
                    Console.WriteLine("User -DisplayName: {0}  UPN: {1}",
                        user.DisplayName, user.UserPrincipalName);
                    if (user.ThumbnailPhoto.ContentType == null) continue;
                    var photo = await user.ThumbnailPhoto.DownloadAsync();
                    if (photo != null) _cache.Add(new Tuple<string, Stream>(user.GivenName, photo.Stream));
                }
                searchResults = await userCollection.GetNextPageAsync();
            } while (searchResults != null);

            return _cache;
        }
    }
}
