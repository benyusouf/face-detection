using System.Drawing;
using System.IO;

namespace FacesApiTest
{
    public class ImageUtility
    {
        public byte[] ConvertToBytes(string imagePath)
        {
            var ms = new MemoryStream();

            using var stream = new FileStream(imagePath, FileMode.Open);

            stream.CopyTo(ms);

            return ms.ToArray();
        }

        public void ConvertToImage(byte[] imageBytes, string fileName)
        {
            using var ms = new MemoryStream();
            var image = Image.FromStream(ms);
            image.Save(fileName + ".jpg");
        }
    }
}
