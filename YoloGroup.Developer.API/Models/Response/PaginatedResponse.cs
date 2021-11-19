using System.Collections.Generic;

namespace YoloGroup.Developer.API.Models.Response
{
    public class PaginatedResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<T> Data { get; set; }
    }
}
