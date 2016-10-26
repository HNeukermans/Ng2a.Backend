using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
    public class IntegrationTests
    {
        [TestMethod]
        public async Task VisionAPI_Should_DetectFaces()
        {

            using (Stream imageFileStream = File.OpenRead("SAMSON_GERT2.JPG"))
            {
                var faces = await new FaceDectector().Detect(imageFileStream);
                Assert.IsTrue(faces.Count() == 1);
            }
        }

        [TestMethod]
        public void VisionAPI_Should_DrawRectangles()
        {
            var fr = new FaceRectangle();
            fr.Left = 0;
            fr.Top = 0;
            fr.Width = 50;
            fr.Height = 50;
            var rectangles = new List<FaceRectangle>() {  };

            using (Stream imageFileStream = File.OpenRead("SAMSON_GERT2.JPG"))
            {
                var outputStream = new SquareDrawer().Draw(imageFileStream, rectangles);
                using (FileStream fs = new FileStream("SAMSON_GERT2_FD.JPG", FileMode.Create))
                {
                    outputStream.Position = 0;
                    outputStream.CopyTo(fs);
                    outputStream.Flush();
                    fs.Flush();
                }
            }
        }

        [TestMethod]
        public void ImageResizer_Should_Resize()
        {
            using (FileStream fs = new FileStream("SAMSON_GERT2.JPG", FileMode.Open))
            {
                var resized = new ImageResizer().Resize(fs);

                using (FileStream rsf = new FileStream("SAMSON_GERT2_RS.JPG", FileMode.Create))
                {
                    resized.CopyTo(rsf);
                }
            }
        }
    }
}
