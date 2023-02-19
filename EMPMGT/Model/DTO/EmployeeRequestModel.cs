using EMPMGT.Model.Tentant;
using System.ComponentModel.DataAnnotations;

namespace EMPMGT.Model.DTO
{
    public class EmployeeRequestModel: TenantDataRecord
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Department { get; set; }
    }
}
