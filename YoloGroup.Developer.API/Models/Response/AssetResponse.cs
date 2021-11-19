namespace YoloGroup.Developer.API.Models.Response
{
    public class AssetResponse
    {
        public Asset[] assets { get; set; }
    }

    public class Asset
    {
        public string assetName { get; set; }
        public string assetSymbol { get; set; }
        public long? marketCap { get; set; }
    }

}
