using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using TranslateAPI.Model;
using Markdig;
using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Net.Http.Headers;

namespace TranslateApi.Controllers
{
    [Route("translate")]
    public class TranslateController : Controller
    {
        static string host = "https://api.cognitive.microsofttranslator.com";
        static string path = "/translate?api-version=3.0";
        static string params_ = "&to=pt&to=en";
        static string uri = host + path;        
        static string key = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        string OriginLanguage = "";

        private string fileName { get; set; }

        [HttpGet("index")]
        public IActionResult Index()
        {
            TranslatedFileViewModel model = new TranslatedFileViewModel();
            model.Languages.AddRange(LanguagesList());

            return View(model);
        }

        private List<SelectListItem> LanguagesList()
        {
            List<SelectListItem> language = new List<SelectListItem>();
            language.Add(new SelectListItem() { Text = "Afrikaans", Value = "af" });
            language.Add(new SelectListItem() { Text = "Arabic", Value = "ar" });
            language.Add(new SelectListItem() { Text = "Arabic, Levantine", Value = "apc" });
            language.Add(new SelectListItem() { Text = "Bangla", Value = "bn" });
            language.Add(new SelectListItem() { Text = "Bosnian (Latin)", Value = "bs" });
            language.Add(new SelectListItem() { Text = "Bulgarian", Value = "bg" });
            language.Add(new SelectListItem() { Text = "Cantonese (Traditional)", Value = "yue" });
            language.Add(new SelectListItem() { Text = "Catalan", Value = "ca" });
            language.Add(new SelectListItem() { Text = "Chinese Simplified", Value = "zh-Hans" });
            language.Add(new SelectListItem() { Text = "Chinese Traditional", Value = "zh-Hant" });
            language.Add(new SelectListItem() { Text = "Croatian", Value = "hr" });
            language.Add(new SelectListItem() { Text = "Czech", Value = "cs" });
            language.Add(new SelectListItem() { Text = "Danish", Value = "da" });
            language.Add(new SelectListItem() { Text = "Dutch", Value = "nl" });
            language.Add(new SelectListItem() { Text = "English", Value = "en" });
            language.Add(new SelectListItem() { Text = "Estonian", Value = "et" });
            language.Add(new SelectListItem() { Text = "Fijian", Value = "fj" });
            language.Add(new SelectListItem() { Text = "Filipino", Value = "fil" });
            language.Add(new SelectListItem() { Text = "Finnish", Value = "fi" });
            language.Add(new SelectListItem() { Text = "French", Value = "fr" });
            language.Add(new SelectListItem() { Text = "German", Value = "de" });
            language.Add(new SelectListItem() { Text = "Greek", Value = "el" });
            language.Add(new SelectListItem() { Text = "Haitian Creole", Value = "ht" });
            language.Add(new SelectListItem() { Text = "Hebrew", Value = "he" });
            language.Add(new SelectListItem() { Text = "Hindi", Value = "hi" });
            language.Add(new SelectListItem() { Text = "Hmong Daw", Value = "mww" });
            language.Add(new SelectListItem() { Text = "Hungarian", Value = "hu" });
            language.Add(new SelectListItem() { Text = "Icelandic", Value = "is" });
            language.Add(new SelectListItem() { Text = "Indonesian", Value = "id" });
            language.Add(new SelectListItem() { Text = "Italian", Value = "it" });
            language.Add(new SelectListItem() { Text = "Japanese", Value = "ja" });
            language.Add(new SelectListItem() { Text = "Kiswahili", Value = "sw" });
            language.Add(new SelectListItem() { Text = "Klingon", Value = "tlh" });
            language.Add(new SelectListItem() { Text = "Klingon (plqaD)", Value = "tlh-Qaak" });
            language.Add(new SelectListItem() { Text = "Korean", Value = "ko" });
            language.Add(new SelectListItem() { Text = "Latvian", Value = "lv" });
            language.Add(new SelectListItem() { Text = "Lithuanian", Value = "lt" });
            language.Add(new SelectListItem() { Text = "Malagasy", Value = "mg" });
            language.Add(new SelectListItem() { Text = "Malay", Value = "ms" });
            language.Add(new SelectListItem() { Text = "Maltese", Value = "mt" });
            language.Add(new SelectListItem() { Text = "Norwegian", Value = "nb" });
            language.Add(new SelectListItem() { Text = "Persian", Value = "fa" });
            language.Add(new SelectListItem() { Text = "Polish", Value = "pl" });
            language.Add(new SelectListItem() { Text = "Portuguese", Value = "pt", Selected = true });
            language.Add(new SelectListItem() { Text = "Queretaro Otomi", Value = "otq" });
            language.Add(new SelectListItem() { Text = "Romanian", Value = "ro" });
            language.Add(new SelectListItem() { Text = "Russian", Value = "ru" });
            language.Add(new SelectListItem() { Text = "Samoan", Value = "sm" });
            language.Add(new SelectListItem() { Text = "Serbian (Cyrillic)", Value = "sr-Cyrl" });
            language.Add(new SelectListItem() { Text = "Serbian (Latin)", Value = "sr-Latn" });
            language.Add(new SelectListItem() { Text = "Slovak", Value = "sk" });
            language.Add(new SelectListItem() { Text = "Slovenian", Value = "sl" });
            language.Add(new SelectListItem() { Text = "Spanish", Value = "es" });
            language.Add(new SelectListItem() { Text = "Swedish", Value = "sv" });
            language.Add(new SelectListItem() { Text = "Tahitian", Value = "ty" });
            language.Add(new SelectListItem() { Text = "Tamil", Value = "ta" });
            language.Add(new SelectListItem() { Text = "Telugu", Value = "te" });
            language.Add(new SelectListItem() { Text = "Thai", Value = "th" });
            language.Add(new SelectListItem() { Text = "Tongan", Value = "to" });
            language.Add(new SelectListItem() { Text = "Turkish", Value = "tr" });
            language.Add(new SelectListItem() { Text = "Ukrainian", Value = "uk" });
            language.Add(new SelectListItem() { Text = "Urdu", Value = "ur" });
            language.Add(new SelectListItem() { Text = "Vietnamese", Value = "vi" });
            language.Add(new SelectListItem() { Text = "Welsh", Value = "cy" });
            language.Add(new SelectListItem() { Text = "Yucatec Maya", Value = "yua" });
            return language;
        }

        private async Task<string> Translate(string text, TranslatedFileViewModel model)
        {
            uri = uri + "&to=" + model.to; 

            System.Object[] body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Translated>>(responseBody);

                var textResult = result.FirstOrDefault().translations.FirstOrDefault(x => x.to.Equals(model.to)).text;
                OriginLanguage = result.FirstOrDefault().detectedLanguage.language;

                return FilterResponse(textResult);
            }
        }

        private string FilterResponse(string text)
        {
            return text.Replace("# #", "##");
        }

        [HttpPost("PostFile")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(IFormFile file, TranslatedFileViewModel model)
        {  
            var pipeline = new Markdig.MarkdownPipelineBuilder().UseAdvancedExtensions().Build();    
            model.Languages.AddRange(LanguagesList());

            var filePath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", 
                        file.FileName);

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                
                var tr = System.IO.File.ReadAllText(filePath);
                System.IO.File.Delete(filePath);

                model.OriginalHtml = Markdown.ToHtml(tr);
                model.OriginalText = tr.Replace("\r\n", "<br>");

                var translatedText = await Translate(tr,model);
                model.TranslatedHtml = Markdown.ToHtml(translatedText, pipeline);
                model.TranslatedText = translatedText.Replace("\r\n", "<br>");

                model.TranslatedPlainText = translatedText;
                model.from = OriginLanguage;

            }

            return View("Index",model);
        }

        [HttpPost("createfile")]
        public async Task<IActionResult> DownloadFile(string text)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            string name = Guid.NewGuid().ToString() + ".md";

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, name)))
            {
                await outputFile.WriteAsync(WebUtility.HtmlDecode(text));
            }

            return Json(new { fileName = name });
        }

        [HttpGet]
        public async Task<ActionResult> Download(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

            var memory = new MemoryStream();

            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/text", Path.GetFileName(path));

        }
    }
}
