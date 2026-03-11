using HRMS.Domain.Entities;

public class OrderCreateDto
{
    public string Phone { get; set; } = string.Empty;
    public List<OrderItemCreateDto> Items { get; set; } = new List<OrderItemCreateDto>();

}