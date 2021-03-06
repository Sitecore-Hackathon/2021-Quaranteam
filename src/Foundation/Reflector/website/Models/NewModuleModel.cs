using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Hackathon.Foundation.Reflector.Models
{
    public class NewModuleModel
    {
        public string ContentTypeName { get; set; }

        public Item PageTemplate { get; set; }

        public List<Item> BaseTemplates { get; set; }
        public Dictionary<string,Item> FieldList { get; set; }
    }
}