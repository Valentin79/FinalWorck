
using Microsoft.EntityFrameworkCore;
using DbApi;

namespace UserApi.DB
{
    public partial class AppDbContext: DbContext
    {
        private string _connectionString;
        public AppDbContext()
        {

        }

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_connectionString);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.ToTable("users");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
                entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Salt).HasColumnName("salt");
                entity.Property(e => e.RoleId).HasConversion<int>();
            });

            modelBuilder
                .Entity<Role>()
                .Property(e => e.RoleId)
                .HasConversion<int>();

            modelBuilder
                .Entity<Role>().HasData(
                Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Select(e => new Role()
                {
                    RoleId = e,
                    Email = e.ToString(),
                }));

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageId);
                entity.ToTable("messages");
                entity.Property(e => e.MessageId).HasColumnName("id");           
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.IsRead).HasColumnName("IsRead");
                entity.Property(e => e.Text).HasMaxLength(700).HasColumnName("text");
                entity.Property(e => e.FromUserId).HasMaxLength(255).HasColumnName("FromUser");
                entity.Property(e => e.ToUserId).HasMaxLength(255).HasColumnName("ToUser");
                
            });
        }
    }
}
