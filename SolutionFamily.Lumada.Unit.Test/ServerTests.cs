﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;

namespace SolutionFamily.Lumada.Unit.Test
{
    [TestClass]
    public class ServerTests
    {
        [TestMethod]
        public void TestServerInfo()
        {
            var server = new Server("192.168.10.212");
            server.IgnoreCertificateErrors = true;
            var info = AsyncHelper.RunSync(() =>
                {
                    return server.GetServerInfoAsync();
                }
            );
            Assert.IsNotNull(info);
        }

        [TestMethod]
        public void TestCreateSession()
        {
            var server = new Server("192.168.10.212");
            server.IgnoreCertificateErrors = true;
            var info = AsyncHelper.RunSync(() =>
            {
                var session = server.CreateSessionAsync("admin", "lumada123!");
                return session;
            }
            );
            Assert.IsNotNull(info);
        }

        [TestMethod]
        public void TestRefreshSession()
        {
            var server = new Server("192.168.10.212");
            server.IgnoreCertificateErrors = true;
            var session = AsyncHelper.RunSync(() =>
            {
                return server.CreateSessionAsync("admin", "lumada123!");
            });
            Assert.IsNotNull(session);

            var exp = session.Expires;

            Thread.Sleep(5000);

            AsyncHelper.RunSync(() =>
            {
                var task = session.RefreshAsync();
                return task;
            });

            var exp2 = session.Expires;

            // make sure the expiration has advanced
            Assert.IsTrue(exp2 > exp);
        }
    }
}
