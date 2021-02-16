using AltairTask.Data;
using AltairTask.Models;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AltairTask.Pages
{
    public class PDFOcrModel : ComponentBase
    {
        [Inject]
        protected PDFCognitiveService cognitiveService { get; set; }
        protected string DetectedTextLanguage;
        protected string imagePreview;
        protected bool loading = false;
        byte[] imageFileBytes;
        const string DefaultStatus = "Maximum size allowed for the The File is 4 MB";
        protected string status = DefaultStatus;

        protected ResultDTO Result = new ResultDTO();

        const int MaxFileSize = 4 * 1024 * 1024; // 4MB
        /// <summary>
        /// Resspond to file uploader event
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        protected async Task ViewImage(IFileListEntry[] files)
        {
            var file = files.FirstOrDefault();
            if (file == null)
            {
                return;
            }
            else if (file.Size > MaxFileSize)
            {
                status = $"The file size is {file.Size} bytes, this is more than the allowed limit of {MaxFileSize} bytes.";
                return;
            }

            else
            {
                var memoryStream = new MemoryStream();
                await file.Data.CopyToAsync(memoryStream);
                imageFileBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(imageFileBytes, 0, imageFileBytes.Length);
                status = DefaultStatus;
            }
        }
        /// <summary>
        /// Respond to sumit button
        /// </summary>
        /// <returns></returns>
        protected private async Task GetText()
        {
            if (imageFileBytes != null)
            {
                loading = true;
                Result = await cognitiveService.GetTextFromFile(imageFileBytes);
                loading = false;
            }

        }
    }
}

