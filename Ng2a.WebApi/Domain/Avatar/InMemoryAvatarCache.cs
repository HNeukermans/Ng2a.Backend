using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.Data.OData;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ng2Aa_demo.Domain.Avatar
{
    public class InMemoryAvatarCache
    {
        private readonly ActiveDirectoryClient _adClientProvider;
        private List<Tuple<string, Stream>> _cache;

        public InMemoryAvatarCache(ActiveDirectoryClient adClientProvider)
        {
            _adClientProvider = adClientProvider;
            _cache = null;
        }

        public async Task<List<Tuple<string, Stream>>> Get() {
            if (_cache != null) return _cache;
            
            _cache = new List<Tuple<string, Stream>>();

            List<IUser> usersList = null;
            IPagedCollection<IUser> searchResults = null;

            IPagedCollection<IUser> userCollection = await _adClientProvider.Users.ExecuteAsync();

            do
            {
                var users = userCollection.CurrentPage.ToList();
                List<Task<BitmapImage>> downloadPhotoTasks = new List<Task<BitmapImage>>();
                int count = 0;
                foreach (IUser user in users)
                {
                    Console.WriteLine("User -DisplayName: {0}  UPN: {1}",
                        user.DisplayName, user.UserPrincipalName);
                    if (user.ThumbnailPhoto.ContentType == null) continue;
                    var photo = await GetUserThumbnailPhotoAsync(user); 
                    if (photo != null) _cache.Add(new Tuple<string, Stream>(user.GivenName, photo));
                }                

                searchResults = await userCollection.GetNextPageAsync();
            } while (searchResults != null);

            return _cache;
        }

        /// <summary>
        /// Get the user's photo.
        /// </summary>
        /// <param name="user">The target user.</param>
        /// <returns></returns>
        /// https://github.com/patbosc/windows10o365calendar/blob/ec146025f0015dc914ec0d8a76d14348c736f9a6/o365calendar/o365calendar/Helpers/UserOperations.cs
        public async Task<Stream> GetUserThumbnailPhotoAsync(IUser user)
        {
            //BitmapImage bitmap = null;
            try
            {
                // The using statement ensures that Dispose is called even if an 
                // exception occurs while you are calling methods on the object.
                using (var dssr = await user.ThumbnailPhoto.DownloadAsync())
                using (var stream = dssr.Stream)
                using (var memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);
                    return memStream;
                    //bitmap = new Image();
                    //await bitmap.(memStream.AsRandomAccessStream());
                }

            }
            catch (ODataException)
            {
                // Something went wrong retrieving the thumbnail photo, so set the bitmap to a default image
                //bitmap = new BitmapImage(new Uri("ms-appx:///assets/UserDefaultSignedIn.png", UriKind.RelativeOrAbsolute));
            }

            return null;
        }
    }
}
