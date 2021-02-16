using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AltairTask.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace AltairTask.Data
{
    public class PDFCognitiveService : IAzureCognitiveService
    {
        /// <summary>
        /// uriBase for Endpoint
        /// </summary>
        private const string uriBase = endpoint + "vision/v2.0/read/core/asyncBatchAnalyze";
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
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(JSONResult);

                foreach (var row in myDeserializedClass.recognitionResults)
                {

                    foreach (var line in row.lines)
                    {
                        sb.Append(line.text);
                        sb.Append(' ');

                    }
                }

                resultDTO.DetectedText = sb.ToString();
                return resultDTO;
            }
            catch (Exception ex)
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
                string operationLocation;
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string requestParameters = "language=unk&detectOrientation=true";
                string uri = uriBase + "?" + requestParameters;
                HttpResponseMessage response;
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                }
                if (response.IsSuccessStatusCode)
                {
                    operationLocation =
                        response.Headers.GetValues("Operation-Location").FirstOrDefault();
                }
                else
                {
                    string errorString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("\n\nResponse:\n{0}\n",
                        JToken.Parse(errorString).ToString());
                    return errorString;
                }
                System.Threading.Thread.Sleep(10000);
                response = await client.GetAsync(operationLocation);
                string contentString = await response.Content.ReadAsStringAsync();
                string result = JToken.Parse(contentString).ToString();
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
