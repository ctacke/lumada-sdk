using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    public class AssetType
    {
        public string AssetTypeID { get; internal set; }
        public int Version { get; internal set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; internal set; }
        public DateTime ModifiedDate { get; internal set; }

        public AssetTemplate Template { get; internal set; }
        public string PictureID { get; internal set; }
    }
}
