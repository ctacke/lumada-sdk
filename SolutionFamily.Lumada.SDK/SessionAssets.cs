using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionFamily.Lumada
{
    public sealed class SessionAssets
    {
        private readonly Session m_session;

        internal SessionAssets(Session session)
        {
            m_session = session;
        }

        public async Task<Asset[]> GetAllAsync()
        {
            var at = await m_session.RequestService.GetAssetsAsync(m_session.AccessToken);
            if (at == null) return null;
            return (from a in at
                    select a.ToAsset())
                   .ToArray();
        }

        public async Task<Asset> GetAsync(string assetID)
        {
            var at = await m_session.RequestService.GetAssetAsync(assetID, m_session.AccessToken);
            return at.ToAsset();
        }

        public async Task<Asset> AddAsync(
            string name, 
            IEnumerable<AssetProperty> properties,
            string assetTypeID = null,
            string gatewayID = null,
            string mappingID = null)
        {
            var request = new AssetRequest()
            {
                Name = name,
                Properties = properties.ToAssetPropertyResponses()
            };

            if (assetTypeID != null)
            {
                request.AssetTypeId = assetTypeID;
            }
            if (gatewayID != null)
            {
                request.GatewayId = gatewayID;
            }
            if (mappingID != null)
            {
                request.MappingId = mappingID;
            }

            var response = await m_session.RequestService.AddAssetAsync(request, m_session.AccessToken);

            return response.ToAsset();
        }

        public async Task UpdatePropertiesAsync(Asset asset, IEnumerable<AssetProperty> newProperties)
        {
            var p = new
            {
                properties = (from ap in newProperties
                              select new AssetPropertyResponse()
                              {
                                  Name = ap.Name,
                                  Value = ap.Value
                              }).ToArray()
            };

            var json = JsonConvert.SerializeObject(p);
            var bytes = Encoding.UTF8.GetBytes(json);
            var encoded = Convert.ToBase64String(bytes, 0, bytes.Length);

            var payload = new
            {
                op = "replace",
                path = "/properties",
                value = encoded
            };
        }

        public async Task DeleteAsync(Asset asset)
        {
            await DeleteAsync(asset.AssetID);
        }

        public async Task DeleteAsync(string assetID)
        {
            await m_session.RequestService.DeleteAssetAsync(assetID, m_session.AccessToken);
        }

        public async Task<AssetToken> GetTokenAsync(Asset asset)
        {
            return await GetTokenAsync(asset.AssetID);
        }

        public async Task<AssetToken> GetTokenAsync(string assetID)
        {
            var response = await m_session.RequestService.GetAssetTokenAsync(assetID, m_session.AccessToken);
            return response.ToAssetToken();
        }
    }
}
