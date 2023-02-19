using EMPMGT.Model.Tentant;
using Microsoft.EntityFrameworkCore;

namespace EMPMGT.DataContext
{
    public partial class EmpContext : DbContext
    {
        protected readonly IConfiguration configuration;
        public EmpContext(IConfiguration configuration)
        {
            this.configuration = configuration;
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("EmpMgtDb");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }




    }
}
