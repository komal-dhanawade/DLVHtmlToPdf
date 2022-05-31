using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NReco_HtmlToPdf.Helpers
{
    public static class GlobalClass
    {

        public static void GetViewMappings()
        {

            List<KeyValuePair<string, string>> ViewMappings = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("E-DLV", ""),
            new KeyValuePair<string, string>("E-CHKLST", ""),
            new KeyValuePair<string, string>("I-DLV", ""),
            new KeyValuePair<string, string>("I-CHKLST", ""),
            new KeyValuePair<string, string>("RE-DLV", ""),
            new KeyValuePair<string, string>("RE-CHKLST", ""),
            new KeyValuePair<string, string>("RI-DLV", ""),
            new KeyValuePair<string, string>("RI-CHKLST", "")
            };
        }
    }
}