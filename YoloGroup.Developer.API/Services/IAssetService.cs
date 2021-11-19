using GraphQL;
using GraphQL.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoloGroup.Developer.API.Models.Response;

namespace YoloGroup.Developer.API.Services
{
    public interface IAssetService
    {
        Task<AssetResponse> GetAssets();
    }

    public class AssetService : IAssetService
    {
        private readonly IGraphQLClient _client;

        public AssetService(IGraphQLClient graphQLClient)
        {
            this._client = graphQLClient;
        }
        public async Task<AssetResponse> GetAssets()
        {
            var query = new GraphQLRequest
            {
                Query = @"query PageAssets {
                                  assets(sort: [{marketCapRank: ASC}]) {
                                    assetName
                                    assetSymbol
                                    marketCap
                                  }
                                }"
            };
            var response = await _client.SendQueryAsync<AssetResponse>(query);
            return response.Data;
        }
    }
}
