using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    internal class AssetResponse
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }
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
        [JsonProperty(PropertyName = "created")]
        public long Created { get; set; }
        [JsonProperty(PropertyName = "modified")]
        public long Modified { get; set; }
    }
}
