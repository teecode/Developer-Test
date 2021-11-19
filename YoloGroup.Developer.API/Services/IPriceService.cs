using GraphQL;
using GraphQL.Client.Abstractions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using YoloGroup.Developer.API.Models.Response;

namespace YoloGroup.Developer.API.Services
{
    public interface IPriceService
    {
        Task<PriceResponse> GetPrices(string[] baseSymbol);
    }

    public class PriceService : IPriceService
    {
        private readonly IGraphQLClient _client;

        public PriceService(IGraphQLClient graphQLClient)
        {
            this._client = graphQLClient;
        }
        public async Task<PriceResponse> GetPrices(string[] baseSymbol)
        {
            var currency = "\"EUR\"";
            var exchangeSymbol = "\"Binance\"";
            var query = new GraphQLRequest
            {
                Query = $@"query price {{
                              markets(filter: {{baseSymbol: {{_in: {JsonConvert.SerializeObject(baseSymbol)}}}, quoteSymbol: {{_eq: {currency} }}, exchangeSymbol: {{_eq: {exchangeSymbol}}}}}) {{
                                marketSymbol
                                ticker {{
                                            lastPrice
                                        }}
                                        }}
                                    }}"
            };
            var response = await _client.SendQueryAsync<PriceResponse>(query);
            return response.Data;
        }
    }
}
