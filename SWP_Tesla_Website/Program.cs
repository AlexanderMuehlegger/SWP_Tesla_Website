using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace SWP_Tesla_Website {
    public class Program : IDistributedCache {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });

        public byte[] Get(string key) {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default) {
            throw new NotImplementedException();
        }

        public void Refresh(string key) {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(string key, CancellationToken token = default) {
            throw new NotImplementedException();
        }

        public void Remove(string key) {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key, CancellationToken token = default) {
            throw new NotImplementedException();
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options) {
            throw new NotImplementedException();
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default) {
            throw new NotImplementedException();
        }
    }
}
