namespace EMPMGT.Model.Tentant
{
    public partial class Employee : TenantDataRecord
    {
        public Employee() { }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string? Department { get; set; }
    }
}
