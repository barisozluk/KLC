using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.Models
{
    public class DbImportExportObjectModel
    {
        public string tableName { get; set; }
        public List<JObject> rows { get; set; }
    }
}
