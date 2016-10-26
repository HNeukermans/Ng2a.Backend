using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Net.Sockets;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Net;
//using Microsoft.ProjectOxford.Face;
//using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford;
using Microsoft.ProjectOxford.Face;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.ProjectOxford.Face.Contract;
using ImplementWebJobs;

namespace ImplementWebJobs.UnitTests
{
    [TestClass]
    public class WebJobIntegrationTests
    {
        [TestMethod]
        public async Task Webjob_Should_DetectFaces()
        {
            using (Stream input = File.OpenRead("SAMSON_GERT2.JPG"))
            {
                var memStream = new MemoryStream();
                await Program.DetectFaces(input, memStream, "SAMSON_GERT2.JGP", new StringWriter());
                using (FileStream output = new FileStream("SAMSON_GERT2_FD_WEBJOB.JPG", FileMode.Create))
                {
                    memStream.Position = 0;
                    memStream.CopyTo(output);
                    output.Flush();
                }
            }
        }

        [TestMethod]
        public void Webjob_Should_CreateThumbnail()
        {
            using (Stream input = File.OpenRead("SAMSON_GERT2.JPG"))
            {
                var memStream = new MemoryStream();
                Program.CreateThumbnail(input, memStream, "SAMSON_GERT2.JGP", new StringWriter());
                using (FileStream output = new FileStream("SAMSON_GERT2_TN_WEBJOB.JPG", FileMode.Create))
                {
                    memStream.Position = 0;
                    memStream.CopyTo(output);
                    output.Flush();
                }
            }
        }           
    }
}
