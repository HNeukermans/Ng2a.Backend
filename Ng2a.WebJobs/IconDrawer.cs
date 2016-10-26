using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementWebJobs
{
    public class SmileyDrawer
    {
        public void Draw(string imageFilePath, List<FaceRectangle> faces, string iconPath)
        {
            if (faces.Any())
            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                MemoryStream outputStream = new MemoryStream();
                using (System.Drawing.Image samsomImage = System.Drawing.Image.FromStream(imageFileStream, true))
                {
                    using (Graphics g = Graphics.FromImage(samsomImage))
                    {
                        Pen yellowPen = new Pen(System.Drawing.Color.Yellow, 4);
                        foreach (FaceRectangle faceRectangle in faces)
                        {
                            g.DrawRectangle(yellowPen,
                                faceRectangle.Left, faceRectangle.Top,
                                faceRectangle.Width, faceRectangle.Height);
                        }
                    }
                    samsomImage.Save(outputStream, ImageFormat.Jpeg);
                }

                using (FileStream fs = new FileStream("SAMSON_GERT2_FD.JPG", FileMode.Create))
                {
                    outputStream.Position = 0;
                    outputStream.CopyTo(fs);
                    outputStream.Flush();
                    fs.Flush();
                }
            }
        }
    }
}
