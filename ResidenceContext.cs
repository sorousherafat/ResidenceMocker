using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;

namespace ResidenceMocker;

public class ResidenceContext : DbContext
{
    public ResidenceContext()
    {
    }

    public ResidenceContext(DbContextOptions<ResidenceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; } = null!;

    public virtual DbSet<Address> Addresses { get; set; } = null!;

    public virtual DbSet<City> Cities { get; set; } = null!;

    public virtual DbSet<Complaint> Complaints { get; set; } = null!;

    public virtual DbSet<DamageReport> DamageReports { get; set; } = null!;

    public virtual DbSet<Guest> Guests { get; set; } = null!;

    public virtual DbSet<Host> Hosts { get; set; } = null!;

    public virtual DbSet<Message> Messages { get; set; } = null!;

    public virtual DbSet<PriceChange> PriceChanges { get; set; } = null!;

    public virtual DbSet<Province> Provinces { get; set; } = null!;

    public virtual DbSet<Rent> Rents { get; set; } = null!;

    public virtual DbSet<RentalRequest> RentalRequests { get; set; } = null!;

    public virtual DbSet<Residence> Residences { get; set; } = null!;

    public virtual DbSet<Review> Reviews { get; set; } = null!;

    public virtual DbSet<Unavailability> Unavailabilities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("bed_type", new[] {"KingSized", "QueenSized", "DoubleBed", "SingleBed"})
            .HasPostgresEnum("building_type", new[] {"Apartment", "Villa", "Hotel-Apartment", "Ecotourism"})
            .HasPostgresEnum("rent_status", new[] {"Cancelled", "Pending", "Completed", "Ongoing"})
            .HasPostgresEnum("rental_request_status", new[] {"Pending", "Cancelled", "Rejected", "Accepted"});

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.NationalId).HasName("account_pkey");

            entity.ToTable("account");

            entity.HasIndex(e => e.PhoneNumber, "account_phonenumber_key").IsUnique();

            entity.Property(e => e.NationalId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("nationalid");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("firstname");
            entity.Property(e => e.JoinedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("joinedat");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("lastname");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("phonenumber");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("address_pkey");

            entity.ToTable("address");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CityId).HasColumnName("cityid");
            entity.Property(e => e.Details)
                .HasMaxLength(200)
                .HasColumnName("details");
            entity.Property(e => e.Geolocation).HasColumnName("geolocation");
            entity.Property(e => e.IsRural).HasColumnName("isrural");
            entity.Property(e => e.Street)
                .HasMaxLength(20)
                .HasColumnName("street");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("zipcode");

            entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("address_cityid_fkey");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("city_pkey");

            entity.ToTable("city");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.ProvinceId).HasColumnName("provinceid");

            entity.HasOne(d => d.Province).WithMany(p => p.Cities)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("city_provinceid_fkey");
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => new {e.Id, Rentid = e.RentId}).HasName("complaint_pkey");

            entity.ToTable("complaint");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.RentId).HasColumnName("rentid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsAccepted)
                .HasDefaultValueSql("false")
                .HasColumnName("isaccepted");
            entity.Property(e => e.Photos).HasColumnName("photos");
            entity.Property(e => e.ResolvedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("resolvedat");
            entity.Property(e => e.Title)
                .HasMaxLength(40)
                .HasColumnName("title");

            entity.HasOne(d => d.Rent).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.RentId)
                .HasConstraintName("complaint_rentid_fkey");
        });

        modelBuilder.Entity<DamageReport>(entity =>
        {
            entity.HasKey(e => new {e.Id, Rentid = e.RentId}).HasName("damagereport_pkey");

            entity.ToTable("damagereport");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.RentId).HasColumnName("rentid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EstimatedCost).HasColumnName("estimatedcost");
            entity.Property(e => e.FinalCost).HasColumnName("finalcost");
            entity.Property(e => e.IsAccepted)
                .HasDefaultValueSql("false")
                .HasColumnName("isaccepted");
            entity.Property(e => e.Photos).HasColumnName("photos");
            entity.Property(e => e.ResolvedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("resolvedat");
            entity.Property(e => e.Title)
                .HasMaxLength(40)
                .HasColumnName("title");

            entity.HasOne(d => d.Rent).WithMany(p => p.DamageReports)
                .HasForeignKey(d => d.RentId)
                .HasConstraintName("damagereport_rentid_fkey");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.NationalId).HasName("guest_pkey");

            entity.ToTable("guest");

            entity.Property(e => e.NationalId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("nationalid");
            entity.Property(e => e.Wallet)
                .HasDefaultValueSql("0")
                .HasColumnName("wallet");

            entity.HasOne(d => d.Account).WithOne(p => p.Guest)
                .HasForeignKey<Guest>(d => d.NationalId)
                .HasConstraintName("guest_nationalid_fkey");
        });

        modelBuilder.Entity<Host>(entity =>
        {
            entity.HasKey(e => e.NationalId).HasName("host_pkey");

            entity.ToTable("host");

            entity.Property(e => e.NationalId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("nationalid");
            entity.Property(e => e.IsVerified)
                .HasDefaultValueSql("false")
                .HasColumnName("isverified");
            entity.Property(e => e.NationalCardImage).HasColumnName("nationalcardimage");
            entity.Property(e => e.VerifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("verifiedat");

            entity.HasOne(d => d.Account).WithOne(p => p.Host)
                .HasForeignKey<Host>(d => d.NationalId)
                .HasConstraintName("host_nationalid_fkey");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("message_pkey");

            entity.ToTable("message");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ReceiverNationalId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("recievernationalid");
            entity.Property(e => e.SenderNationalId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("sendernationalid");
            entity.Property(e => e.SentAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sentat");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.ReceiverAccount).WithMany()
                .HasForeignKey(d => d.ReceiverNationalId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("message_recievernationalid_fkey");

            entity.HasOne(d => d.SenderAccount).WithMany()
                .HasForeignKey(d => d.SenderNationalId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("message_sendernationalid_fkey");
        });

        modelBuilder.Entity<PriceChange>(entity =>
        {
            entity.HasKey(e => new {e.Id, Residenceid = e.ResidenceId}).HasName("pricechange_pkey");

            entity.ToTable("pricechange");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ResidenceId).HasColumnName("residenceid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.EndTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("endtime");
            entity.Property(e => e.Factor)
                .HasPrecision(4, 2)
                .HasColumnName("factor");
            entity.Property(e => e.StartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("starttime");

            entity.HasOne(d => d.Residence).WithMany(p => p.PriceChanges)
                .HasForeignKey(d => d.ResidenceId)
                .HasConstraintName("pricechange_residenceid_fkey");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("province_pkey");

            entity.ToTable("province");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rent_pkey");

            entity.ToTable("rent");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CancellationPenalty).HasColumnName("cancellationpenalty");
            entity.Property(e => e.CancellationPolicy)
                .HasColumnType("jsonb")
                .HasColumnName("cancellationpolicy");
            entity.Property(e => e.CancellationTimestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("cancellationtimestamp");
            entity.Property(e => e.FinalPrice).HasColumnName("finalprice");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Request).WithOne(p => p.Rent)
                .HasForeignKey<Rent>(d => d.Id)
                .HasConstraintName("rent_id_fkey");
        });

        modelBuilder.Entity<RentalRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rentalrequest_pkey");

            entity.ToTable("rentalrequest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.EndDate).HasColumnName("enddate");
            entity.Property(e => e.GuestNationalId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("guestnationalid");
            entity.Property(e => e.GuestsNo).HasColumnName("guestsno");
            entity.Property(e => e.RawPrice).HasColumnName("rawprice");
            entity.Property(e => e.ResidenceId).HasColumnName("residenceid");
            entity.Property(e => e.ResolvedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("resolvedat");
            entity.Property(e => e.StartDate).HasColumnName("startdate");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Guest).WithMany(p => p.RentalRequests)
                .HasForeignKey(d => d.GuestNationalId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("rentalrequest_guestnationalid_fkey");

            entity.HasOne(d => d.Residence).WithMany(p => p.RentalRequests)
                .HasForeignKey(d => d.ResidenceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("rentalrequest_residenceid_fkey");
        });

        modelBuilder.Entity<Residence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("residence_pkey");

            entity.ToTable("residence");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("addressid");
            entity.Property(e => e.AllPhotos).HasColumnName("allphotos");
            entity.Property(e => e.Area)
                .HasPrecision(6, 2)
                .HasColumnName("area");
            entity.Property(e => e.CancellationPolicy)
                .HasColumnType("jsonb")
                .HasColumnName("cancellationpolicy");
            entity.Property(e => e.Capacity)
                .HasDefaultValueSql("1")
                .HasColumnName("capacity");
            entity.Property(e => e.CheckInTime).HasColumnName("checkintime");
            entity.Property(e => e.CheckOutTime).HasColumnName("checkouttime");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Facility).HasColumnName("facility");
            entity.Property(e => e.HasParking)
                .HasDefaultValueSql("false")
                .HasColumnName("hasparking");
            entity.Property(e => e.HasWifi)
                .HasDefaultValueSql("false")
                .HasColumnName("haswifi");
            entity.Property(e => e.HostNationalId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("hostnationalid");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PrimaryPhoto).HasColumnName("primaryphoto");
            entity.Property(e => e.RentFee)
                .HasDefaultValueSql("0")
                .HasColumnName("rentfee");
            entity.Property(e => e.RoomsNo)
                .HasDefaultValueSql("0")
                .HasColumnName("roomsno");
            entity.Property(e => e.Title)
                .HasMaxLength(20)
                .HasColumnName("title");
            entity.Property(e => e.BuildingType).HasColumnName("buildingtype");
            entity.Property(e => e.BedsType).HasColumnName("bedstype");

            entity.HasOne(d => d.Address).WithMany(p => p.Residences)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("residence_addressid_fkey");

            entity.HasOne(d => d.Host).WithMany(p => p.Residences)
                .HasForeignKey(d => d.HostNationalId)
                .HasConstraintName("residence_hostnationalid_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("review_pkey");

            entity.ToTable("review");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IsByHost).HasColumnName("isbyhost");
            entity.Property(e => e.RentId).HasColumnName("rentid");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Rent).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.RentId)
                .HasConstraintName("review_rentid_fkey");
        });

        modelBuilder.Entity<Unavailability>(entity =>
        {
            entity.HasKey(e => new {e.Id, Residenceid = e.ResidenceId}).HasName("unavailability_pkey");

            entity.ToTable("unavailability");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ResidenceId).HasColumnName("residenceid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.EndTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("endtime");
            entity.Property(e => e.StartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("starttime");

            entity.HasOne(d => d.Residence).WithMany(p => p.Unavailabilities)
                .HasForeignKey(d => d.ResidenceId)
                .HasConstraintName("unavailability_residenceid_fkey");
        });
    }
}