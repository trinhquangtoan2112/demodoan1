using Google.Cloud.TextToSpeech.V1;
using Newtonsoft.Json.Linq;
using System.Text;

using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NAudio.Wave;
namespace demodoan1.Helpers
{
    public class TextToSpeechService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string ApiKey = "AIzaSyD-kRo6DwaDzVeHgu919Ucpk4tLsHrwzBY";
        private const string BucketName = "webdoctruyendoan";
        private const string LongAudioEndpoint = "https://texttospeech.googleapis.com/v1beta1/text:synthesizeLongAudio?key=" + ApiKey;
        //public async Task<byte[]> SynthesizeTextToSpeechAsync(string text)
        //{
        //    var requestUrl = "https://texttospeech.googleapis.com/v1/text:synthesize?key=AIzaSyD-kRo6DwaDzVeHgu919Ucpk4tLsHrwzBY";
        //    var payload = new
        //    {

        //        input = new { text = text },
        //        voice = new { languageCode = "vi-VN", name = "vi-VN-Standard-A" },
        //        audioConfig = new { audioEncoding = "MP3",
        //            pitch= 0,
        //            speakingRate=1
        //        }
        //    };

        //    var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        //    var response = await client.PostAsync(requestUrl, content);
        //    response.EnsureSuccessStatusCode();

        //    var responseData = await response.Content.ReadAsStringAsync();
        //    var result = System.Text.Json.JsonDocument.Parse(responseData);
        //    var audioContent = result.RootElement.GetProperty("audioContent").GetString();
        //    return Convert.FromBase64String(audioContent);
        //}
        private const int MaxByteLength = 4500;
        public async Task<byte[]> SynthesizeTextToSpeechAsync(string text)
        {
            var audioParts = new List<MemoryStream>();
            var parts = SplitTextIntoChunks(text, MaxByteLength);

            foreach (var part in parts)
            {
                var audioPart = await SynthesizePart(part);
                audioParts.Add(new MemoryStream(audioPart));
            }

            return CombineAudioParts(audioParts);
        }

        private IEnumerable<string> SplitTextIntoChunks(string text, int maxByteLength)
        {
            var chunks = new List<string>();
            int start = 0;

            while (start < text.Length)
            {
               
                int length = Math.Min(text.Length - start, 1000);
                var chunk = text.Substring(start, length);

               
                if (Encoding.UTF8.GetByteCount(chunk) > maxByteLength)
                {
                 
                    length = FindSuitableLength(chunk, maxByteLength);
                    chunk = text.Substring(start, length);
                }

                chunks.Add(chunk);
                start += length;
            }

            return chunks;
        }

        private int FindSuitableLength(string text, int maxByteLength)
        {
            int length = text.Length;
            while (length > 0 && Encoding.UTF8.GetByteCount(text.Substring(0, length)) > maxByteLength)
            {
                length--;
            }
            return length;
        }

        private async Task<byte[]> SynthesizePart(string text)
        {
            var requestUrl = $"https://texttospeech.googleapis.com/v1/text:synthesize?key={ApiKey}";
            var payload = new
            {
                input = new { text = text },
                voice = new { languageCode = "vi-VN", name = "vi-VN-Standard-A" },
                audioConfig = new { audioEncoding = "MP3", pitch = 0, speakingRate = 1 }
            };

            var json = JObject.FromObject(payload).ToString();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error: {response.StatusCode}, Content: {errorContent}");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseData);
            var audioContent = result["audioContent"].ToString();
            return Convert.FromBase64String(audioContent);
        }

        private byte[] CombineAudioParts(List<MemoryStream> audioParts)
        {
            using (var output = new MemoryStream())
            {
                foreach (var part in audioParts)
                {
                    part.CopyTo(output);
                }
                return output.ToArray();
            }
        }

    }
}
