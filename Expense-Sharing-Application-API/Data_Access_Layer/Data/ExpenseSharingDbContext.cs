using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Data
{
    public class ExpenseSharingDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Expense> Expenses { get; set; }
   

        public ExpenseSharingDbContext(DbContextOptions<ExpenseSharingDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<Group>()
                .HasKey(g => g.GroupId);

            modelBuilder.Entity<Group>()
                .Property(g => g.GroupId)
                .ValueGeneratedOnAdd(); // Ensure GroupId is an identity column

            modelBuilder.Entity<GroupMember>()
                .HasKey(gm => gm.GroupMemberId); // Configure GroupMemberId as the key

            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(gm => gm.GroupId);

            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.GroupMembers)
                .HasForeignKey(gm => gm.UserId);

            modelBuilder.Entity<Expense>()
                .HasKey(e => e.ExpenseId);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.PaidBy)
                .WithMany()
                .HasForeignKey(e => e.PaidById);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Group)
                .WithMany(g => g.Expenses)
                .HasForeignKey(e => e.GroupId);

            // Apply the User configuration
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
