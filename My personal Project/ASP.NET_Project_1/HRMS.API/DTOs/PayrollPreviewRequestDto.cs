namespace HRMS.API.DTOs
{
    public class PayrollPreviewRequestDto
    {
        public int ? UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}