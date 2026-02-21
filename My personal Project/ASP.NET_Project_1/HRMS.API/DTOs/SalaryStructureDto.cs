using HRMS.Domain.Entities;

public class SalaryStructureDto
{

    public int SalaryStructureId { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }

    public decimal BasicSalary { get; set; }
    public decimal HouseRentAllowance { get; set; }
    public decimal MedicalAllowance { get; set; }

    public decimal TransportAllowance { get; set; }

    public decimal otherAllowance { get; set; }

    public DateOnly ?  EffectiveFrom { get; set; }
    public DateOnly? EffectiveTo { get; set; }

    public bool IsActive { get; set; } = true;

    public Users? Users { get; set; } = null!;


}