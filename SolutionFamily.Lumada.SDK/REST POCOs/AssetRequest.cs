using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    internal class AssetRequest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "gatewayId")]
        public string GatewayId { get; set; }
        [JsonProperty(PropertyName = "mappingId")]
        public string MappingId { get; set; }
        [JsonProperty(PropertyName = "assetTypeId")]
        public string AssetTypeId { get; set; }
        [JsonProperty(PropertyName = "properties")]
        public AssetPropertyResponse[] Properties { get; set; }
    }
}
