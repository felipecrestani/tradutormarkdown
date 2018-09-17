using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TranslateAPI.Model
{
    public class TranslatedFileViewModel
    {
        public TranslatedFileViewModel()
        {
            this.Languages = new List<SelectListItem>();
        }
        public string from { get; set; }
        public string OriginalText { get; set; }
        public string OriginalHtml { get; set; }
        public string to { get; set; }
        public string TranslatedText { get; set; }

        public string TranslatedPlainText { get; set; }
        public string TranslatedHtml { get; set; }

        public string Score { get; set; }

        public List<SelectListItem> Languages  { get; set; }
    }
}
