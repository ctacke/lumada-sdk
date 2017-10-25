using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionFamily.Lumada
{
    public sealed class SessionAssetTypes
    {
        private Session m_session;

        internal SessionAssetTypes(Session session)
        {
            m_session = session;
        }

        public async Task<AssetType[]> GetAllAsync()
        {
            var at = await m_session.RequestService.GetAssetTypesAsync(m_session.AccessToken);
            return (from a in at
                    select new AssetType()
                    {
                        AssetTypeID = a.AssetTypeID,
                        Version = a.Version,
                        CreateDate = a.Created.ToDateTimeFromEpochMilliseconds(),
                        ModifiedDate = a.Modified.ToDateTimeFromEpochMilliseconds(),
                        Template = a.Template == null ? null : JsonConvert.DeserializeObject<AssetTemplate>(a.Template),
                        PictureID = a.PictureID
                    })
                   .ToArray();
        }

        public async Task<AssetType> AddAsync(string name, AssetTemplate template)
        {
            var templateJson = JsonConvert.SerializeObject(template);
            var bytes = Encoding.UTF8.GetBytes(templateJson);
            var encoded = Convert.ToBase64String(bytes, 0, bytes.Length);

            var request = new AssetTypeRequest()
            {
                Name = name,
                Template = encoded,
                // TODO:  PictureID
            };

            var response = await m_session.RequestService.AddAssetTypeAsync(request, m_session.AccessToken);
            return new AssetType()
            {
                AssetTypeID = response.AssetTypeID,
                Version = response.Version,
                CreateDate = response.Created.ToDateTimeFromEpochMilliseconds(),
                ModifiedDate = response.Modified.ToDateTimeFromEpochMilliseconds(),
                Template = JsonConvert.DeserializeObject<AssetTemplate>(response.Template),
                PictureID = response.PictureID
            };
        }

        public async Task DeleteAsync(AssetType assetType)
        {
            await DeleteAsync(assetType.AssetTypeID);
        }

        public async Task DeleteAsync(string assetTypeID)
        {
            await m_session.RequestService.DeleteAssetTypeAsync(assetTypeID, m_session.AccessToken);
        }
    }
}
