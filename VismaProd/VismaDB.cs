namespace VismaProd
{
    using System.Data.Entity;

    public partial class VismaDB : DbContext
    {
        public VismaDB()
            : base("name=VismaDB")
        {
        }

        public virtual DbSet<FreeLancer> FreeLancers { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<TimeInfo> TimeInfoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FreeLancer>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<FreeLancer>()
                .HasMany(e => e.TimeInfoes)
                .WithRequired(e => e.FreeLancer)
                .HasForeignKey(e => e.UID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.Info)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.TimeInfoes)
                .WithRequired(e => e.Project)
                .HasForeignKey(e => e.PID)
                .WillCascadeOnDelete(false);
        }
    }
}
