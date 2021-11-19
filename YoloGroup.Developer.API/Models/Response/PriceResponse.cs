namespace YoloGroup.Developer.API.Models.Response
{
    public class PriceResponse
    {
        public Market[] markets { get; set; }
    }

    public class Market
    {
        public string marketSymbol { get; set; }
        public Ticker ticker { get; set; }
    }

    public class Ticker
    {
        public string lastPrice { get; set; }
    }

}
