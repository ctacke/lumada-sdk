using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace SolutionFamily.Lumada.Unit.Test
{
    [TestClass]
    public class AssetTests
    {
        [TestMethod]
        public void TestGetAssets()
        {
            var server = new Server("192.168.10.212");
            server.IgnoreCertificateErrors = true;
            var session = AsyncHelper.RunSync(() =>
                {
                    return server.CreateSessionAsync("admin", "lumada123!");
                }
            );
            var info = AsyncHelper.RunSync(() =>
            {
                var types = session.Assets.GetAllAsync();
                return types;
            });
            Assert.IsNotNull(info);
        }

        [TestMethod]
        public void TestAddAsset()
        {
            var server = new Server("192.168.10.212");
            server.IgnoreCertificateErrors = true;
            var session = AsyncHelper.RunSync(() =>
            {
                return server.CreateSessionAsync("admin", "lumada123!");
            });

            var asset = AsyncHelper.RunSync(() =>
            {
                return session.Assets.AddAsync("Test Asset",
                new AssetProperty[]
                {
                    new AssetProperty()
                    {
                         Name = "Color",
                         Value = "blue"
                    }
                });
            });

            var info = AsyncHelper.RunSync(() =>
            {
                var types = session.Assets.GetAllAsync();
                return types;
            });
            Assert.IsNotNull(info);
        }

        [TestMethod]
        public void TestGetAssetToken()
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


        }
    }
}
