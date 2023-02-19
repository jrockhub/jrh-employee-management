namespace EMPMGT.Model.Tentant
{
    public class TenantDataRecord
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreationDate { get; set; }

        public TenantDataRecord()
        {
            CreationDate = DateTime.UtcNow;
        }

    }
}
