
namespace Pustaksathi.Model.Application.Members
{
#region Members Orders
  public record Orders
  {
      public Guid OrderId { get; set; }
      public Guid UserId { get; set; }
      public DateTime? OrderDate { get; set; }
      public string? Status { get; set; }
      public string? ClaimCode { get; set; }
      public bool IsCancelled { get; set; }
      public bool LoyalityDiscount { get; set; }
      public bool QuantityDiscount { get; set; } = false;
      public decimal TotalAmount { get; set; }
      public DateTime? CreatedAt { get; set; }
      public DateTime? ModifiedAt { get; set; }
      public IReadOnlyCollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
  }

  public record OrderItems
  {
      public Guid OrderItemId { get; set; }
      public Guid OrderId { get; set; }
      public Guid BookId { get; set; }
      public string? BookTitle { get; set; }
      public int Quantity { get; set; }
      public decimal UnitPrice { get; set; }
  }
  #endregion
}
