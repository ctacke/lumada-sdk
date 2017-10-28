﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFamily.Lumada
{
    public class AssetProperty
    {
        public AssetProperty()
        {
        }

        public AssetProperty(string propertyName, string propertyValue)
        {
            Name = propertyName;
            Value = propertyValue;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
