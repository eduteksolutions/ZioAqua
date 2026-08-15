using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using zioAqua.model;

namespace zioAqua.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<ContainerTransaction> ContainerTransaction { get; set; }

        public DbSet<ContainerTransactionDetail> ContainerTransactionDetail { get; set; }

        public DbSet<AccGroupMaster> AccGroupMaster { get; set; }

        public DbSet<AccLedgerMaster> AccLedgerMaster { get; set; }

        public DbSet<StoreItemMaster> StoreItemMaster { get; set; }

        public DbSet<StoreItemGroup> StoreItemGroup { get; set; }
        public DbSet<BusinessMaster> BusinessMaster { get; set; }

        public DbSet<EventOrderMaster> EventOrderMasters { get; set; }
        public DbSet<EventOrderDetail> EventOrderDetails { get; set; }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }
    }
}