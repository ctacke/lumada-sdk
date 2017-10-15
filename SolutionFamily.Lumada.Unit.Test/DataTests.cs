using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace SolutionFamily.Lumada.Unit.Test
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void TestPublishAssetData()
        {
            var server = new Server("192.168.10.212");
            server.IgnoreCertificateErrors = true;
            var session = AsyncHelper.RunSync(() =>
            {
                return server.CreateSessionAsync("admin", "lumada123!");
            }
            );
            var assets = AsyncHelper.RunSync(() =>
            {
                return session.Assets.GetAllAsync();
            });
            Assert.IsNotNull(assets);

            var first = assets.FirstOrDefault();
            var token = AsyncHelper.RunSync(() =>
            {
                return session.Assets.GetTokenAsync(first);
            });
            Assert.IsNotNull(token);

            AsyncHelper.RunSync(() =>
            {
                return session.Data.UploadAssetStateAsync(
                    first.AssetID,
                    token,
                    new
                    {
                        test_data = 123.45
                    });
            });

            AsyncHelper.RunSync(() =>
            {
                var data = new Dictionary<string, object>();
                data.Add("test1", "foo");
                data.Add("test2", 42.3);

                return session.Data.UploadAssetStateAsync(
                    first.AssetID,
                    token,
                    data);
            });        
        }
    }
}
