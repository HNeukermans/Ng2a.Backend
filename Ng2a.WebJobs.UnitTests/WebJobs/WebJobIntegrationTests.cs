using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.IO;

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
