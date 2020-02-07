using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WatchGuardNASAPhotos.Models;

namespace WatchGuardNASAPhotos.Services
{
    public class PhotoService : IPhotoService
    {
        private const string apiKey = "hT2la1Q3tCiflZMTrAjnUxIZTuQTTMAuJHOSnKF8";
        private HttpClient _client;

        public PhotoService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Photo>> GetPhotos()
        {
            List<DateTime> dates = GetDates();
            List<Photo> returnPhotos = new List<Photo>();
            
            foreach(DateTime dt in dates)
            {
                var httpResponse = await _client.GetAsync("https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?earth_date=" + dt.ToString("yyyy-MM-dd") + "&api_key=" + apiKey);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ToString() + httpResponse.Content.ToString());
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                List<Photo> photos = CustomDeserialization(JObject.Parse(content));

                returnPhotos.AddRange(photos);
            }

            DownloadPhotos(returnPhotos);

            return returnPhotos;
        }

        private List<DateTime> GetDates()
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime dateValue;
            string line;

            FileStream fileStream = new FileStream(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\wwwroot\dates.txt", FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while((line = reader.ReadLine()) != null)
                {
                    if(DateTime.TryParse(line, out dateValue))
                    {
                        dates.Add(dateValue);
                    }
                    else
                    {
                        Console.WriteLine("Unable to parse '{0}'", line);
                    }
                }
            }
            fileStream.Close();
            fileStream.Dispose();

            return dates;
        }

        private List<Photo> CustomDeserialization(JObject jObject)
        {
            List<JToken> results = jObject["photos"].Children().ToList();
            List<Photo> photos = new List<Photo>();
            foreach(JToken result in results)
            {
                Photo photo = result.ToObject<Photo>();
                photos.Add(photo);
            }

            return photos;
        }

        private void DownloadPhotos(List<Photo> photos)
        {
            foreach(Photo p in photos)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(p.img_src), @"c:\temp\" + p.id + ".jpeg");
                }
            }
        }
    }
}
