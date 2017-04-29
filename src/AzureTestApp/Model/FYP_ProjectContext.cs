using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace AzureTestApp.Model
{
    public partial class FYP_ProjectContext : DbContext
    {
        public FYP_ProjectContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.            

            optionsBuilder.UseSqlServer(@"Data Source=dominicbrennan.database.windows.net;Initial Catalog=FYP_Project;Persist Security Info=True;User ID=dominicbrennan;Password=Fld118yi;;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;");

            //Data Source=dominicbrennan.database.windows.net;Initial Catalog=FYP_Project;Persist Security Info=True;User ID=dominicbrennan;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Column Encryption Setting=Enabled
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guardians>(entity =>
            {
                entity.HasKey(e => e.GuardianId)
                    .HasName("PK_guardians");

                entity.ToTable("guardians");

                entity.Property(e => e.GuardianId).HasColumnName("GuardianID");
            });

            modelBuilder.Entity<NotePatient>(entity =>
            {
                entity.HasKey(e => new { e.NoteId, e.PpsNo })
                    .HasName("PK_notePatient");

                entity.ToTable("notePatient");

                entity.HasIndex(e => e.NoteId)
                    .HasName("AK_notePatient_NoteID")
                    .IsUnique();

                entity.Property(e => e.NoteId).HasColumnName("NoteID");

                entity.Property(e => e.PpsNo).HasColumnName("PPS_No");
            });

            modelBuilder.Entity<Notes>(entity =>
            {
                entity.HasKey(e => e.NoteId)
                    .HasName("PK_notes");

                entity.ToTable("notes");

                entity.Property(e => e.NoteId).HasColumnName("NoteID");

                entity.Property(e => e.NoteDate).HasColumnType("date");

                entity.Property(e => e.PpsNo).HasColumnName("PPS_No");
            });

            modelBuilder.Entity<OtTasks>(entity =>
            {
                entity.HasKey(e => e.OtTaskId)
                    .HasName("PK_otTasks");

                entity.ToTable("otTasks");

                entity.Property(e => e.DueDate).HasColumnType("datetime2(1)");

                entity.Property(e => e.OccId).HasColumnName("OccID");

                entity.Property(e => e.PpsNo).HasColumnName("PPS_No");

                entity.Property(e => e.TaskId).HasColumnName("TaskID");
            });

            modelBuilder.Entity<PatientAccount>(entity =>
            {
                entity.HasKey(e => e.PpsNo)
                    .HasName("PK_patientAccount");

                entity.ToTable("patientAccount");

                entity.Property(e => e.PpsNo).HasColumnName("PPS_No");

                entity.Property(e => e.GuardianId).HasColumnName("GuardianID");

                entity.Property(e => e.OccId).HasColumnName("OccID");

                entity.Property(e => e.OpenDate).HasDefaultValueSql("'0001-01-01T00:00:00.000'");

                entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

                entity.Property(e => e.SocialSecurityNo).HasColumnName("Social_Security_No");
            });

            modelBuilder.Entity<SchoolLists>(entity =>
            {
                entity.HasKey(e => e.SchoolId)
                    .HasName("PK_schoolLists");

                entity.ToTable("schoolLists");

                entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("PK_tasks");

                entity.ToTable("tasks");

                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.Property(e => e.DueDate).HasColumnType("datetime2(1)");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_userAccount");

                entity.ToTable("userAccount");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });
        }

        public virtual DbSet<Guardians> Guardians { get; set; }
        public virtual DbSet<NotePatient> NotePatient { get; set; }
        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<OtTasks> OtTasks { get; set; }
        public virtual DbSet<PatientAccount> PatientAccount { get; set; }
        public virtual DbSet<SchoolLists> SchoolLists { get; set; }
        public virtual DbSet<Tasks> TaskName { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
    }
}