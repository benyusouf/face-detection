using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;

namespace FacesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacesController : ControllerBase
    {
        [HttpPost]
        public async Task<IList<byte[]>> ReadFaces()
        {
            using var ms = new MemoryStream(2040);

            await Request.Body.CopyToAsync(ms);

            return GetFaces(ms.ToArray());
        }

        private IList<byte[]> GetFaces(byte[] image)
        {
            var src = Mat.ImDecode(image, ImreadModes.Color);

            src.SaveImage("image.jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));

            var file = Path.Combine(Directory.GetCurrentDirectory(), "CascadeFile", "haarcascade_frontalface_default.xml");
            var faceCascade = new CascadeClassifier();
            faceCascade.Load(file);
            var faces = faceCascade.DetectMultiScale(src, 1.1, 6, HaarDetectionTypes.DoRoughSearch, new Size(60, 60));

            var facesList = new List<byte[]>();
            var j = 0;
            foreach(var rect in faces)
            {
                var face = new Mat(src, rect);
                facesList.Add(face.ToBytes(".jpg"));
                face.SaveImage("face" + j + ".jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));
                j++;
            }

            return facesList;
        }
    }
}