using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.ProjectOxford.Face.Contract;
using System.IO;

namespace ImplementWebJobs
{
    public class FaceDectector
    {
        public async Task<List<FaceRectangle>> Detect(Stream input)
        {
            MemoryStream copy = CreateWorkingCopy(input);
            var faceRectangles = new List<FaceRectangle>();
            var key = Environment.GetEnvironmentVariable("Vision_API_Subscription_Key", EnvironmentVariableTarget.User);
            var faceServiceClient = new FaceServiceClient(key);
            var faces = await faceServiceClient.DetectAsync(copy);
            faceRectangles = faces.Select(face => face.FaceRectangle).ToList();

            return faceRectangles;
        }

        private static MemoryStream CreateWorkingCopy(Stream input)
        {
            var copy = new MemoryStream();
            input.CopyTo(copy);
            copy.Position = 0;
            input.Position = 0;
            return copy;
        }
    }
}
