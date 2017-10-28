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
            if (responseProps == null) return null;

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
            if (properties == null) return null;

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
            if (a == null) return null;

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
            if (response == null) return null;

            return new AssetToken()
            {
                ID = response.ID,
                Token = response.Token,
                AuthHash = response.AuthHash
            };
        }

        public static AssetType ToAssetType(this AssetTypeResponse response)
        {
            if (response == null) return null;

            return new AssetType()
            {
                AssetTypeID = response.AssetTypeID,
                Version = response.Version,
                Name = response.Name,
                CreateDate = response.Created.ToDateTimeFromEpochMilliseconds(),
                ModifiedDate = response.Modified.ToDateTimeFromEpochMilliseconds(),
                Template = response.Template == null ? null : JsonConvert.DeserializeObject<AssetTemplate>(response.Template),
                PictureID = response.PictureID
            };
        }
    }
}
