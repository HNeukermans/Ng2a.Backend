using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using irp = ImageResizer;

namespace ImplementWebJobs
{
    public class ImageResizer
    {
        public Stream Resize(Stream image) {

            var memStream = new MemoryStream();
            var i = new irp.ImageJob(image, memStream,
                new irp.Instructions("width=50;height=50;format=jpg;mode=max"));
            i.Build();
            memStream.Position = 0;
            return memStream;
        }
    }
}
