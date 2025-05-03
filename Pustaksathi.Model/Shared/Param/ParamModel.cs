
namespace Pustaksathi.Model.Shared.Param
{
    public record MvReqOptionParam<T>
    {
        public T? Filter { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; } 
        public int OffSet { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
}
