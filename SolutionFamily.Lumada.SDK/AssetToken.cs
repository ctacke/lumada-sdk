using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionFamily.Lumada
{
    public class AssetToken
    {
        internal AssetToken()
        {
        }

        public string ID { get; internal set; }
        public string Token { get; internal set; }
        public string AuthHash { get; internal set; }
    }
}
