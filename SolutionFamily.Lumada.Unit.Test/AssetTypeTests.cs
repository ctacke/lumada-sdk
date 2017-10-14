using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace SolutionFamily.Lumada.Unit.Test
{
    [TestClass]
    public class AssetTypeTests
    {
        [TestMethod]
        public void TestGetAssetTypes()
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
                var types = session.AssetTypes.GetAllAsync();
                return types;
            }
            );
            Assert.IsNotNull(info);
        }

        [TestMethod]
        public void TestCreateAssetType()
        {
            var server = new Server("192.168.10.212");
            server.IgnoreCertificateErrors = true;
            var session = AsyncHelper.RunSync(() =>
            {
                return server.CreateSessionAsync("admin", "lumada123!");
            }
            );

            var p = new List<AssetTemplateProperty>();
            p.Add(new AssetTemplateProperty()
            {
                Name = "TestInt",
                Type = "int",
                Description = "Test integer"
            });

            var template = new AssetTemplate()
            {
                Name = "Test",
                DisplayName = "Test Asset",
                Description = "Unit test created template",
                BaseType = "Entity",
                Properties = p
            };

            var info = AsyncHelper.RunSync(() =>
            {
                var type = session.AssetTypes.AddAsync("SFTest2", template);

                return type;
            });
            Assert.IsNotNull(info);

            AsyncHelper.RunSync(() =>
            {
                return session.AssetTypes.DeleteAsync(info.AssetTypeID);
            });
            Assert.IsNotNull(info);

        }
    }
}
