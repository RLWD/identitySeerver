using System;
using VDW.SalesApp.Common.Redis;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            RedisDataCache cache = new RedisDataCache("dev-salesapp.redis.cache.windows.net:6380,password=P6WAGR3FvO3ZaZZzvXD0KYrVKQzbv2v4YAzCaL2sYMI=,ssl=True,abortConnect=False");
            _ = cache.Set<string>("a", "cde").Result;
            var s = cache.Get<string>("a").Result;
            Assert.True(s == "a");
        }
    }
}
