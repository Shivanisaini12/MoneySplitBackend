using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DomainLayer.Data;

public partial class RxSplitterContext : DbContext
{
    public RxSplitterContext()
    {
    }

    public RxSplitterContext(DbContextOptions<RxSplitterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountSetting> AccountSettings { get; set; }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<DbError> DbErrors { get; set; }

    public virtual DbSet<DigestiveEmailPeriod> DigestiveEmailPeriods { get; set; }

    public virtual DbSet<DigestiveEmailType> DigestiveEmailTypes { get; set; }

    public virtual DbSet<DigestiveEmailUserSetting> DigestiveEmailUserSettings { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<ExpenseTransaction> ExpenseTransactions { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupMember> GroupMembers { get; set; }

    public virtual DbSet<MemberInvitation> MemberInvitations { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<SettleUp> SettleUps { get; set; }

    public virtual DbSet<Summary> Summaries { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<UserAccountSetting> UserAccountSettings { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.100.193\\MSSQLSERVER2019;Database=RxSplitter;Persist Security Info=False;User ID=satish.jangid;Password=Radixweb8;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<AccountSetting>(entity =>
        {
            entity.ToTable("AccountSetting");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Activity__3214EC0768A28EEF");

            entity.ToTable("ActivityLog");

            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.LogDatetime).HasColumnType("datetime");
            entity.Property(e => e.RequestBody).IsUnicode(false);
            entity.Property(e => e.RequestHost).IsUnicode(false);
            entity.Property(e => e.RequestMethod).IsUnicode(false);
            entity.Property(e => e.RequestScheme).IsUnicode(false);
            entity.Property(e => e.RequestUrl)
                .IsUnicode(false)
                .HasColumnName("RequestURL");
            entity.Property(e => e.ResponseBody).IsUnicode(false);
            entity.Property(e => e.ResponseMessage).IsUnicode(false);
            entity.Property(e => e.ResponseStatus).IsUnicode(false);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Connection>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currency__3214EC07E44DEE3D");

            entity.ToTable("Currency");

            entity.Property(e => e.Code).HasMaxLength(3);
            entity.Property(e => e.Icon).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Symbol).HasMaxLength(5);
        });

        modelBuilder.Entity<DbError>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DB_Errors");

            entity.Property(e => e.ErrorDateTime).HasColumnType("datetime");
            entity.Property(e => e.ErrorId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ErrorID");
            entity.Property(e => e.ErrorMessage).IsUnicode(false);
            entity.Property(e => e.ErrorProcedure).IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DigestiveEmailPeriod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Digestiv__3214EC07F4928CE7");

            entity.ToTable("DigestiveEmailPeriod");

            entity.Property(e => e.EmailTypeName).IsUnicode(false);
        });

        modelBuilder.Entity<DigestiveEmailType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Digestiv__3214EC07459A370F");

            entity.ToTable("DigestiveEmailType");

            entity.Property(e => e.EmailType).IsUnicode(false);
        });

        modelBuilder.Entity<DigestiveEmailUserSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Digestiv__3214EC07E4DDA701");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.EmailPeriod).WithMany(p => p.DigestiveEmailUserSettings)
                .HasForeignKey(d => d.EmailPeriodId)
                .HasConstraintName("FK__Digestive__Email__1B9317B3");

            entity.HasOne(d => d.EmailType).WithMany(p => p.DigestiveEmailUserSettings)
                .HasForeignKey(d => d.EmailTypeId)
                .HasConstraintName("FK__Digestive__Email__1C873BEC");

            entity.HasOne(d => d.User).WithMany(p => p.DigestiveEmailUserSettings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Digestive__UserI__19AACF41");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ExpenseNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Expenses_Category");
        });

        modelBuilder.Entity<ExpenseTransaction>(entity =>
        {
            entity.ToTable("ExpenseTransaction");

            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.TransactionNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Expenses).WithMany(p => p.ExpenseTransactions)
                .HasForeignKey(d => d.ExpensesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpenseTransaction_Expenses");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group");

            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrencyId).HasDefaultValueSql("((0))");
            entity.Property(e => e.GroupName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValueSql("('Default Group')");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<GroupMember>(entity =>
        {
            entity.ToTable("GroupMember");

            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupMember_Group");

            entity.HasOne(d => d.User).WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_GroupMember_UserDetail");
        });

        modelBuilder.Entity<MemberInvitation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MemberIn__3214EC073D35D998");

            entity.ToTable("MemberInvitation");

            entity.Property(e => e.InvitationStatus).HasMaxLength(2);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.TokenGeneratedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.MemberInvitations)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvGroupId");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberInvitations)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberId");

            entity.HasOne(d => d.TokenGeneratedByUserNavigation).WithMany(p => p.MemberInvitations)
                .HasForeignKey(d => d.TokenGeneratedByUser)
                .HasConstraintName("FK_Inv_UserId");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Details).HasMaxLength(500);
            entity.Property(e => e.DetailsUrl)
                .HasMaxLength(500)
                .HasColumnName("DetailsURL");
            entity.Property(e => e.NotificationType).HasMaxLength(100);
            entity.Property(e => e.SentTo).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<SettleUp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SettleUp__3214EC07A8A02B01");

            entity.ToTable("SettleUp");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.SettleupDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.SettleUps)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupId");

            entity.HasOne(d => d.PayerMember).WithMany(p => p.SettleUpPayerMembers)
                .HasForeignKey(d => d.PayerMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PMemberId");

            entity.HasOne(d => d.RecipientMember).WithMany(p => p.SettleUpRecipientMembers)
                .HasForeignKey(d => d.RecipientMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RMemberId");
        });

        modelBuilder.Entity<Summary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Summary__3214EC07F35B8489");

            entity.ToTable("Summary");

            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.RemainingAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.Summaries)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SumGroupId");

            entity.HasOne(d => d.Participant).WithMany(p => p.Summaries)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParticipantId");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transaction");

            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("((0))")
                .IsFixedLength();
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserAccountSetting>(entity =>
        {
            entity.ToTable("UserAccountSetting");

            entity.HasOne(d => d.AccountSetting).WithMany(p => p.UserAccountSettings)
                .HasForeignKey(d => d.AccountSettingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAccou__Accou__7755B73D");

            entity.HasOne(d => d.User).WithMany(p => p.UserAccountSettings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserAccou__UserI__7849DB76");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserDetail_1");

            entity.ToTable("UserDetail");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AddedOn).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
