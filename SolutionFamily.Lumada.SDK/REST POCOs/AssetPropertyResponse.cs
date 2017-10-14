using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    internal class AssetPropertyResponse
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
