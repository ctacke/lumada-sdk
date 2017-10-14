using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    internal class AssetTypeResponseEnvelope
    {
        [JsonProperty(PropertyName = "contents")]
        public AssetTypeResponse[] AssetTypes { get; set; }
    }

    internal class AssetTypeResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string AssetTypeID { get; set; }
        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }
        [JsonProperty(PropertyName = "template")]
        public string Template { get; set; }
        [JsonProperty(PropertyName = "pictureId")]
        public string PictureID { get; set; }
        [JsonProperty(PropertyName = "created")]
        public long Created { get; set; }
        [JsonProperty(PropertyName = "modified")]
        public long Modified { get; set; }
    }
}
