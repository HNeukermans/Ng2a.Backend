using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.IO;
using nQuant;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace ImplementWebJobs
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {            
            var host = new JobHost();
            //The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();         
        }
        public static void CreateThumbnail(
            [BlobTrigger("images-original/{fileName}")] Stream original,
            [Blob("images-processed/thumbnail/{fileName}", FileAccess.Write)] Stream thumbnail, string fileName, TextWriter logger)
        {
            logger.WriteLine("Creating thumbnail for file '{0}' ...", fileName);
            var result = new ImageResizer().Resize(original);
            result.CopyTo(thumbnail);
            logger.WriteLine("Thumbnail created.");
        }

        public static async Task DetectFaces(
           [BlobTrigger("images-original/{fileName}")] Stream original,
           [Blob("images-processed/faces-detected/{fileName}", FileAccess.Write)] Stream workingFile, string fileName, TextWriter logger)
        {
            logger.WriteLine("Starting face detection for file '{0}'...", fileName);
            var rects = await new FaceDectector().Detect(original);
            logger.WriteLine("Faces detected. Start drawing rectangles...");
            var result = new SquareDrawer().Draw(original, rects);
            result.CopyTo(workingFile);
            logger.WriteLine("Face rectangles drawing finished.");
        }

        //private void DetectFaces() {
        //    MemoryStream mem = new MemoryStream();
        //    image.CopyTo(mem); //make a copy since one gets destroy in the other API. Lame, I know.
        //    image.Position = 0;
        //    mem.Position = 0;

        //    string result = await CallVisionAPI(image);
        //    log.Info(result);

        //    if (String.IsNullOrEmpty(result))
        //    {
        //        return req.CreateResponse(HttpStatusCode.BadRequest);
        //    }

        //    ImageData imageData = JsonConvert.DeserializeObject<ImageData>(result);

        //    MemoryStream outputStream = new MemoryStream();
        //    using (Image maybeFace = Image.FromStream(mem, true))
        //    {
        //        using (Graphics g = Graphics.FromImage(maybeFace))
        //        {
        //            Pen yellowPen = new Pen(Color.Yellow, 4);
        //            foreach (Face face in imageData.Faces)
        //            {
        //                var faceRectangle = face.FaceRectangle;
        //                g.DrawRectangle(yellowPen,
        //                    faceRectangle.Left, faceRectangle.Top,
        //                    faceRectangle.Width, faceRectangle.Height);
        //            }
        //        }
        //        maybeFace.Save(outputStream, ImageFormat.Jpeg);
        //    }

        //    var response = new HttpResponseMessage()
        //    {
        //        Content = new ByteArrayContent(outputStream.ToArray()),
        //        StatusCode = HttpStatusCode.OK,
        //    };
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        //    return response;
        //}

        //static async Task<string> CallVisionAPI(Stream image)
        //{
        //    var key = Environment.GetEnvironmentVariable("Vision_API_Subscription_Key");
        //    IFaceServiceClient faceServiceClient = new FaceServiceClient("Your subscription key");
        //    var faceRectangles = new List<FaceRectangle>();
        //    using (Stream imageFileStream = File.OpenRead(imageFilePath))
        //    {
        //        var faces = await faceServiceClient.DetectAsync(imageFileStream);
        //        var faceRects = faces.Select(face => face.FaceRectangle);
        //        faceRectangles = faceRects.ToList();
        //    }

        //    if (faceRectangles.Any()) {
        //        MemoryStream outputStream = new MemoryStream();
        //        using (Image maybeFace = Image.FromStream(mem, true))
        //        {
        //            using (Graphics g = Graphics.FromImage(maybeFace))
        //            {
        //                Pen yellowPen = new Pen(Color.Yellow, 4);
        //                foreach (FaceRectangle faceRect in faceRectangles)
        //                {
        //                    var faceRectangle = faceRect.FaceRectangle;
        //                    g.DrawRectangle(yellowPen,
        //                        faceRectangle.Left, faceRectangle.Top,
        //                        faceRectangle.Width, faceRectangle.Height);
        //                }
        //            }
        //            maybeFace.Save(outputStream, ImageFormat.Jpeg);
        //        }
        //    }


        //    //using (var client = new HttpClient())
        //    //{
        //    //    var content = new StreamContent(image);
        //    //    var url = "https://api.projectoxford.ai/vision/v1.0/analyze?visualFeatures=Faces";
        //    //    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Vision_API_Subscription_Key"));
        //    //    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //    //    var httpResponse = await client.PostAsync(url, content);

        //    //    if (httpResponse.StatusCode == HttpStatusCode.OK)
        //    //    {
        //    //        return await httpResponse.Content.ReadAsStringAsync();
        //    //    }
        //    //}
        //    return null;
        //}


        //public static void MakeThumbNails(
        //    [BlobTrigger("images2/original/{fileName}")] Stream original,
        //    [Blob("images2/thumbnail/{fileName}", FileAccess.Write)] Stream thumbnail, string fileName, string ext, TextWriter logger)
        //{
        //    logger.WriteLine("try processing " + fileName);
        //    if (fileName.Contains("thumbnail")) return;
        //    original.CopyTo(thumbnail);

        //    QuantizeImage(original, thumbnail);
        //}

        //private static void QuantizeImage(Stream original, Stream thumbnail)
        //{
        //    var quantizer = new WuQuantizer();
        //    using (var bitmap = new System.Drawing.Bitmap(original))
        //    {
        //        using (var quantized = quantizer.QuantizeImage(bitmap))
        //        {
        //            quantized.Save(thumbnail, ImageFormat.Png);
        //        }
        //    }
        //}
    }
}
