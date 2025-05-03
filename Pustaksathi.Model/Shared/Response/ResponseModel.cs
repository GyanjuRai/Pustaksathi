


namespace Pustaksathi.Model.Shared.Response
{
    public record ResponseModel<T>
    {
        public required string Message { get; set; }
        public required string Type { get; set; }
        public  T? Data { get; set; }
        public Exception? Exception { get; set; }
    }

    public record GridResponse<T>
    {
        public required int TotalRows { get; set; }
        public T? Data { get; set; }
    }

    public record AttributeItemResponse
    {
        public required int AttributeId { get; set; }
        public required string ItemName { get; set; }
        public required string ItemValue { get; set; }
    }

    public record LoginResponseModel
    {
        public required Guid UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
