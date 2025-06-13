namespace Domain.Entities;

public class SearchCriteria
{
    public int Id { get; set; }
    public string Keywords { get; set; } = string.Empty;
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string Location { get; set; } = string.Empty ;
    public Guid UserId { get; set; }
}