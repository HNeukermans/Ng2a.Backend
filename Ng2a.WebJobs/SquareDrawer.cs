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
    public class SquareDrawer
    {
        public Stream Draw(Stream input, List<FaceRectangle> rectangles)
        {
            MemoryStream outputStream = new MemoryStream();

            using (System.Drawing.Image workingImage = System.Drawing.Image.FromStream(input, true))
            {
                using (Graphics g = Graphics.FromImage(workingImage))
                {
                    Pen yellowPen = new Pen(System.Drawing.Color.Yellow, 4);
                    foreach (FaceRectangle rectangle in rectangles)
                    {
                        g.DrawRectangle(yellowPen,
                            rectangle.Left, rectangle.Top,
                            rectangle.Width, rectangle.Height);
                    }
                }
                workingImage.Save(outputStream, ImageFormat.Jpeg);
            }                
            
            outputStream.Position = 0;
            return outputStream;
        }
    }
}
