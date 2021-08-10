using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FacesApiTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var imagePath = @"oscars-2017.jpg";
            var endpointUrl = "http://localhost:5000/api/faces";
            var imageUtil = new ImageUtility();
            var bytes = imageUtil.ConvertToBytes(imagePath);
            List<byte[]> faceList = null;
            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using var httpClient = new HttpClient();
            using var response = await httpClient.PostAsync(endpointUrl, byteContent);
            var apiResponse = await response.Content.ReadAsStringAsync();

            faceList = JsonConvert.DeserializeObject<List<byte[]>>(apiResponse);

            if(faceList.Count > 0)
            {
                for (int i = 0; i < faceList.Count; i++)
                {
                    imageUtil.ConvertToImage(faceList[i], "face" + i);
                }
            }
        }
    }
}
