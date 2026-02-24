using HRMS.Domain.Entities;

public class SalaryAdjustmentDto
{
    public int UserId { get; set; }
    //public string UserName { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }

    public List<BonusDto> Bonuses { get; set; }
    public List<DeductionDto> Deductions { get; set; }

}