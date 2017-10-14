using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    public class Asset
    {
        public string AssetID { get; internal set; }
        public int Version { get; internal set; }
        public string Name { get; internal set; }
        public string GatewayID { get; internal set; }
        public string MappingID { get; internal set; }
        public string AssetTypeID { get; internal set; }
        public AssetProperty[] Properties { get; internal set; }
        public DateTime CreateDate { get; internal set; }
        public DateTime ModifiedDate { get; internal set; }
    }
}
