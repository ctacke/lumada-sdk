using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionFamily.Lumada
{
    internal static class Extensions
    {
        public static AssetProperty[] ToAssetProperties(this AssetPropertyResponse[] responseProps)
        {
            var result = (from p in responseProps
                          select new AssetProperty()
                          {
                              Name = p.Name,
                              Value = p.Value
                          }).ToArray();

            return result;
        }

        public static AssetPropertyResponse[] ToAssetPropertyResponses(this IEnumerable<AssetProperty> properties)
        {
            var result = (from p in properties
                          select new AssetPropertyResponse()
                          {
                              Name = p.Name,
                              Value = p.Value
                          }).ToArray();

            return result;
        }

        public static Asset ToAsset(this AssetResponse a)
        {
            return (new Asset()
            {
                AssetID = a.ID,
                Name = a.Name,
                GatewayID = a.GatewayId,
                MappingID = a.MappingId,
                AssetTypeID = a.AssetTypeId,
                Version = a.Version,
                CreateDate = a.Created.ToDateTimeFromEpochMilliseconds(),
                ModifiedDate = a.Modified.ToDateTimeFromEpochMilliseconds(),
                Properties = a.Properties.ToAssetProperties()
            });
        }

        public static AssetToken ToAssetToken(this AssetTokenResponse response)
        {
            return new AssetToken()
            {
                ID = response.ID,
                Token = response.Token,
                AuthHash = response.AuthHash
            };
        }
    }
}
