using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using YoloGroup.Developer.API.Services;

namespace YoloGroup.Developer.Test
{
    public class QuestionServiceTests
    {
        private QuestionService _questionService;
        private readonly Mock<ICacheManager> _cacheManager = new Mock<ICacheManager>();
        private readonly Mock<IAssetService> _assetService = new Mock<IAssetService>();
        private readonly Mock<IPriceService> _priceService = new Mock<IPriceService>();

        [SetUp]
        public void Setup()
        {
            _questionService = new QuestionService(_cacheManager.Object, _priceService.Object);

            _cacheManager.Setup(x => x.AllAssets(false)).Returns(Task.FromResult(new API.Models.Response.AssetResponse
            {
                assets = new API.Models.Response.Asset[]
                {
                    new API.Models.Response.Asset{ assetName = "Bitcoin", assetSymbol= "BTC", marketCap=1075521958278},
                    new API.Models.Response.Asset{ assetName = "Ethereum", assetSymbol= "ETH", marketCap=490186719242},
                    new API.Models.Response.Asset{ assetName = "Binance Coin", assetSymbol= "BNB", marketCap=93622208943}
                }
            }));

            _priceService.Setup(x => x.GetPrices(It.IsAny<string[]>())).Returns(Task.FromResult(new API.Models.Response.PriceResponse
            {
                markets = new API.Models.Response.Market[] {
                    new API.Models.Response.Market{marketSymbol = "Binance:BTC/EUR", ticker= new API.Models.Response.Ticker { lastPrice = "42515.34000000" } },
                    new API.Models.Response.Market{marketSymbol = "Binance:ETH/EUR", ticker= new API.Models.Response.Ticker { lastPrice = "3326.33000000" } },
                    new API.Models.Response.Market{marketSymbol = "Binance:BNB/EUR", ticker= new API.Models.Response.Ticker { lastPrice = "414.90000000" } }
                    }
            }));
        }

        [Test]
        public void InverseText_WhenCalled_ReturnsInversedText()
        {
            var testWord = "I love timi";
            var serviceresponse = _questionService.inverseText(testWord);
            Assert.AreEqual("timi love I", serviceresponse);
        }

        [Test]
        public async Task GetPaginatedAssetPrices_WhenCalled_ReturnsAmountAndPrices()
        {
            var serviceresponse = await _questionService.GetPaginatedAssetPrices(1, 3);
            Assert.NotNull(serviceresponse);
            Assert.AreEqual(serviceresponse.Total, 3);
        }
    }
}