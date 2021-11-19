using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using YoloGroup.Developer.API.Models.Response;

namespace YoloGroup.Developer.API.Services
{
    public interface ICacheManager
    {
        Task<AssetResponse> AllAssets(bool reload = false);
    }

    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _cache;
        private readonly IAssetService _assetService;

        public CacheManager(IMemoryCache memoryCache, IAssetService assetService)
        {
            this._cache = memoryCache;
            this._assetService = assetService;
        }
        public async Task<AssetResponse> AllAssets(bool reload = false)
        {
            if (reload)
            {
                return _cache.Set("ASSETS", (await _assetService.GetAssets()));
            }
            else
                return (await _cache.GetOrCreateAsync("ASSETS", entry =>
                {
                    entry.SlidingExpiration = TimeSpan.MaxValue;
                    return _assetService.GetAssets();
                }));
        }
    }
}
