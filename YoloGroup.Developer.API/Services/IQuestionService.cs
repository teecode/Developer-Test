using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using YoloGroup.Developer.API.Models.Response;

namespace YoloGroup.Developer.API.Services
{
    public interface IQuestionService
    {
        string inverseText(string text);

        void ProcessConcurrent();
        string LoadAndHashFile();

        Task<PaginatedResponse<AssetPriceResponse>> GetPaginatedAssetPrices(int page = 1, int pageSize = 20);
    }


    public class QuestionService : IQuestionService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IPriceService _priceService;

        public QuestionService(ICacheManager cacheManager, IPriceService priceService)
        {
            this._cacheManager = cacheManager;
            this._priceService = priceService;
        }
        public string inverseText(string text) => text.Split(" ").Reverse().Aggregate((a, b) => $"{a} {b}");

        public string LoadAndHashFile()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Files/100MB.bin");
            SHA256 Sha256 = SHA256.Create();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var hashed = Sha256.ComputeHash(stream);
                var result = BitConverter.ToString(hashed).Replace("-", "");
                return result;
            }
        }

        public void ProcessConcurrent()
        {
            List<Task> _tasks = new List<Task>();
            for (int count = 1; count <= 1000; count++)
            {
                _tasks.Add(ProcessSingle(count));
            }
            Task.WhenAll(_tasks).Wait();
        }

        private async Task ProcessSingle(int item)
        {
            await Task.Delay(100);
            Console.WriteLine($"completes item {item}");
        }


        public async Task<PaginatedResponse<AssetPriceResponse>> GetPaginatedAssetPrices(int page = 1, int pageSize = 20)
        {
            var assets = (await _cacheManager.AllAssets())?.assets.Take(100);
            int start = (page - 1) * pageSize;
            var pagedAsset = assets.Skip(start).Take(pageSize);
            var baseSymbols = pagedAsset.Select(x => x.assetSymbol).Distinct().ToArray();
            var prices = await _priceService.GetPrices(baseSymbols);
            List<AssetPriceResponse> assetPriceResponses = new List<AssetPriceResponse>();
            foreach (var asset in pagedAsset)
            {
                var price = prices.markets.SingleOrDefault(x => x.marketSymbol == $"Binance:{asset.assetSymbol}/EUR");
                assetPriceResponses.Add(new AssetPriceResponse { Asset = asset, Market = price });
            }

            return new PaginatedResponse<AssetPriceResponse> { Data = assetPriceResponses, Page = page, PageSize = pageSize, Total = (assets?.Count() ?? 0) };


        }




    }
}
