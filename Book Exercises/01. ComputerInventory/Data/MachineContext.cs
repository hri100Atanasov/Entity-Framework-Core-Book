using _01._ComputerInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace _01._ComputerInventory.Data
{
    class MachineContext : DbContext
    {
        public virtual DbSet<Machine> Machine { get; set; }
        public virtual DbSet<MachineType> MachineType { get; set; }
        public virtual DbSet<MachineWarranty> MachineWarranty { get; set; }
        public virtual DbSet<OperatingSys> OperatingSys { get; set; }
        public virtual DbSet<SupportLog> SupportLog { get; set; }
        public virtual DbSet<SupportTicket> SupportTicket { get; set; }
        public virtual DbSet<WarrantyProvider> WarrantyProvider { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-5H5JRQ7;Database=BookEFCore;Trusted_Connection=false;User ID=Iko;Password=");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Machine>(entity =>
            {
                entity.Property(e => e.MachineId).HasColumnName("MachineID");
                entity.Property(e => e.GeneralRole).IsRequired().HasMaxLength(25).IsUnicode(false);
                entity.Property(e => e.InstalledRolls).IsRequired().HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(25).IsUnicode(false);

                entity.Property(e => e.MachineTypeId).HasColumnName("MachineTypeID");
                entity.Property(e => e.OperatingSysId).HasColumnName("OperatingSysID");

                entity.HasOne(d => d.MachineType).WithMany(p => p.Machine)
                .HasForeignKey(d => d.MachineTypeId).OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MachineType");

                entity.HasOne(d => d.OperatingSys).WithMany(p => p.Machine)
                .HasForeignKey(d => d.OperatingSysId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperatingSys");
            });

            modelBuilder.Entity<MachineType>(entity =>
            {
                entity.Property(e => e.MachineTypeId).HasColumnName("MachineTypeID");

                entity.Property(e => e.Description).HasMaxLength(15).IsUnicode(false);
            });

            modelBuilder.Entity<MachineWarranty>(entity =>
            {
                entity.Property(e => e.MachineWarrantyId).HasColumnName("MachineWarrantyID");
                entity.Property(e => e.MachineId).HasColumnName("MachineID");

                entity.Property(e => e.ServiceTag).IsRequired().HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.WarrantyExpiration).HasColumnType("date");

                entity.Property(e => e.WarrantyProviderId).HasColumnName("WarrantyProviderID");

                entity.HasOne(d => d.WarrantyProvider).WithMany(p => p.MachineWarranty)
                .HasForeignKey(d => d.WarrantyProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarrantyProvider");
            });

            modelBuilder.Entity<OperatingSys>(entity =>
            {
                entity.Property(e => e.OperatingSysId).HasColumnName("OperatingSysID");

                entity.Property(e => e.Name).IsRequired().IsUnicode(false).HasMaxLength(35);
            });

            modelBuilder.Entity<SupportLog>(entity =>
            {
                entity.Property(e => e.SupportLogId).HasColumnName("SupportLogID");
                entity.Property(e => e.SupportTicketId).HasColumnName("SupportTicketID");

                entity.Property(e => e.SupportLogEntry).IsRequired().IsUnicode(false);
                entity.Property(e => e.SupportLogEntryDate).HasColumnType("date");
                entity.Property(e => e.SupportLogUpdatedBy).IsRequired().IsUnicode(false).HasMaxLength(50);

                entity.HasOne(d => d.SupportTicket).WithMany(p => p.SupportLog)
                .HasForeignKey(d => d.SupportTicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupportTicket");
            });

            modelBuilder.Entity<SupportTicket>(entity =>
            {
                entity.Property(e => e.SupportTicketId).HasColumnName("SupportTicketID");
                entity.Property(e => e.DateReported).HasColumnType("date");
                entity.Property(e => e.DateResolved).HasColumnType("date");
                entity.Property(e => e.IssueDescription).IsRequired().IsUnicode(false).HasMaxLength(150);
                entity.Property(e => e.IssueDetail).IsUnicode(false);
                entity.Property(e => e.MachineId).HasColumnName("MachineID");
                entity.Property(e => e.TicketOpendBy).IsRequired().HasMaxLength(50).IsUnicode(false);

                entity.HasOne(d => d.Machine).WithMany(p => p.SupportTicket)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Machine");
            });

            modelBuilder.Entity<WarrantyProvider>(entity =>
            {
                entity.Property(e => e.WarrantyProviderId).HasColumnName("WarrantyProviderID");
                entity.Property(e => e.ProviderName).IsRequired().HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.SupportNumber).IsRequired().HasMaxLength(10).IsUnicode(false);
            });
        }

    }
}
