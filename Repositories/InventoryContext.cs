using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace DAS_DESAFIO_DOS_INVENTARIO.Repositories
{
    public class InventoryContext : DbContext
    {

        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id"); // Si también usas id en lugar de Id.
                entity.Property(e => e.RoleId).HasColumnName("role_id"); // Si también usas id en lugar de Id.
                entity.Navigation(u => u.Role).AutoInclude();

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
            });
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Relación uno a muchos con usuarios
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        // Clave foránea a Role
        public int RoleId { get; set; }

        [JsonIgnore]
        public Role Role { get; set; }

        // Relación uno a muchos con productos
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 0;

        // Clave foránea a User
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }

    public class NewProduct
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 0;
    }
}
