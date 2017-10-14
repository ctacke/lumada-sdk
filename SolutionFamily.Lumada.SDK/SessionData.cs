using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionFamily.Lumada
{
    public class SessionData
    {
        private Session m_session;

        internal SessionData(Session session)
        {
            m_session = session;
        }

        public async Task UploadAssetState(Asset asset, AssetToken token, object stateData)
        {
            await UploadAssetState(asset.AssetID, token, stateData);
        }

        public async Task UploadAssetState(string assetID, AssetToken token, object stateData)
        {
            await m_session.RequestService.UploadAssetState(assetID, token, stateData);
        }
    }
}
