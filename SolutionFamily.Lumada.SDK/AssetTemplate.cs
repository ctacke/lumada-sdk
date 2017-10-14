using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    public class AssetTemplate
    {
        public string Name { get; set; }
        public string BaseType { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public List<AssetAnnotation> Annotations { get; set; }
        public List<AssetTemplateProperty> Properties { get; set; }
    }
}
