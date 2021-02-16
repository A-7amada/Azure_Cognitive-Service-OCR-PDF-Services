using AltairTask.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AltairTask.Data
{
    /// <summary>
    /// Ocr Cognitive Service
    /// </summary>
    public class OCRCognitiveService : IAzureCognitiveService
    {
        /// <summary>
        /// uriBase for Endpoint
        /// </summary>
        private const string uriBase = endpoint + "vision/v2.1/ocr";
        /// <summary>
        /// Deserialize Json result and return the result DTO 
        /// </summary>
        /// <param name="imageFileBytes"></param>
        /// <returns></returns>
        public override async Task<ResultDTO> GetTextFromFile(byte[] imageFileBytes)
        {
            StringBuilder sb = new StringBuilder();
            ResultDTO resultDTO = new ResultDTO();
            try
            {
                string JSONResult = await ReadTextFromStream(imageFileBytes);

                OcrResult ocrResult = JsonConvert.DeserializeObject<OcrResult>(JSONResult);
                if (!ocrResult.Language.Equals("unk"))
                {
                    foreach (OcrLine ocrLine in ocrResult.Regions[0].Lines)
                    {
                        foreach (OcrWord ocrWord in ocrLine.Words)
                        {
                            sb.Append(ocrWord.Text);
                            sb.Append(' ');
                        }
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.Append("This language is not supported.");
                }
                resultDTO.DetectedText = sb.ToString();
                resultDTO.Language = ocrResult.Language;
                return resultDTO;
            }
            catch
            {
                resultDTO.DetectedText = "Error occurred. Try again";
                resultDTO.Language = "unk";
                return resultDTO;
            }
        }
        /// <summary>
        /// Call Endpoint and get Recohnized Text
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public override async Task<string> ReadTextFromStream(byte[] byteData)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string requestParameters = "language=unk&detectOrientation=true";
                string uri = uriBase + "?" + requestParameters;
                HttpResponseMessage response;
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                }
                string contentString = await response.Content.ReadAsStringAsync();
                string result = JToken.Parse(contentString).ToString();
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        /// Get List of Current Supported Lanaguage
        /// </summary>
        /// <returns></returns>
        public async Task<AvailableLanguage> GetAvailableLanguages()
        {
            string endpoint = "https://api.cognitive.microsofttranslator.com/languages?api-version=3.0&scope=translation";
            var client = new HttpClient();
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(endpoint);
                var response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();
                AvailableLanguage deserializedOutput = JsonConvert.DeserializeObject<AvailableLanguage>(result);
                return deserializedOutput;
            }
        }

    }
}
