using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Microsoft.Net.Http.Headers;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;

namespace Ng2Aa_demo.Controllers
{
    [Route("api/[controller]")]
    public class AvatarsController : Controller
    {
        private static List<Tuple<string,Stream>> _cachedImages = new List<Tuple<string, Stream>>();
                
        //[HttpGet("{user}")]
        public HttpResponseMessage Get(string user)
        {
            PopulateCache();

            var userImage = _cachedImages.Find(t => t.Item1 == user);
            if (userImage == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(userImage.Item2);
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return result;
        }

        private void PopulateCache() {
            if (_cachedImages.Count > 0) return;
            //FetchAvatars();
        }

        //private static async Task FetchAvatars()
        //{
        //    try
        //    {
        //        client = AuthenticationHelper.GetActiveDirectoryClientAsApplication();
        //    }
        //    catch (Exception e)
        //    {
        //        //TODO: Implement retry and back-off logic per the guidance given here:http://msdn.microsoft.com/en-us/library/dn168916.aspx
        //        Program.WriteError("Acquiring a token failed with the following error: {0}",
        //            Program.ExtractErrorMessage(e));
        //        return;
        //    }

        //    List<IUser> usersList = null;
        //    IPagedCollection<IUser> searchResults = null;
        //    try
        //    {
        //        IPagedCollection<IUser> userCollection = await client.Users.ExecuteAsync();

        //        do
        //        {
        //            var users = userCollection.CurrentPage.ToList();
        //            foreach (IUser user in users)
        //            {
        //                Console.WriteLine("User -DisplayName: {0}  UPN: {1}",
        //                    user.DisplayName, user.UserPrincipalName);
        //                if (user.ThumbnailPhoto.ContentType == null) continue;
        //                var photo = await user.ThumbnailPhoto.DownloadAsync();
        //                if (photo != null) SavePicture(user.GivenName, photo);
        //            }
        //            searchResults = await userCollection.GetNextPageAsync();
        //        } while (searchResults != null);

        //    }
        //    catch (Exception e)
        //    {
        //        Program.WriteError("\nError getting Avatars {0}", Program.ExtractErrorMessage(e));
        //    }
        //}
    }
}
