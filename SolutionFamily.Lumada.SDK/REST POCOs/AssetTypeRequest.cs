using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    internal class AssetTypeRequest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "pictureId")]
        public string PictureID { get; set; }
        [JsonProperty(PropertyName = "template")]
        public string Template { get; set; }
    }
}
