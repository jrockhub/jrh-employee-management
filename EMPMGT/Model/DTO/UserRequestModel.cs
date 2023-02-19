using EMPMGT.Model.Tentant;

namespace EMPMGT.Model.DTO
{
    public class UserRequestModel: TenantDataRecord
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

       
    }
}
