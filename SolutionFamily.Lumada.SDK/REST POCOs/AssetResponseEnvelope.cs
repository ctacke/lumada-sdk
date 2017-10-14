using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    internal class AssetResponseEnvelope
    {
        [JsonProperty(PropertyName = "continuationToken")]
        public string ContinuationToken { get; set; }

        [JsonProperty(PropertyName = "contents")]
        public AssetResponse[] Assets { get; set; }
    }
}
