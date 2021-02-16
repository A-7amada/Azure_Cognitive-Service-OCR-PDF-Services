using AltairTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltairTask.Data
{
    /// <summary>
    /// Abstract class for Azure Cognitive Service
    /// </summary>
    public abstract class IAzureCognitiveService
    {
        //Insert your Cognitive Services subscription key here
        public const string subscriptionKey = "8246f25e02ce4832b61dabde704a344c";
        // You must use the same Azure region that you generated your subscription keys for.   
        public const string endpoint = "https://altairalfeditask.cognitiveservices.azure.com/";
        /// <summary>
        /// Get Recognized Text From File
        /// </summary>
        /// <param name="imageFileBytes"></param>
        /// <returns></returns>
        public abstract Task<ResultDTO> GetTextFromFile(byte[] imageFileBytes);
        /// <summary>
        /// Read Text From Stream
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public abstract Task<string> ReadTextFromStream(byte[] byteData);

    }
}
