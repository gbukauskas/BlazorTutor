//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBtransitions.ClassLibraryDatabase.CustomFilter;


namespace WEBtransitions.ClassLibraryDatabase.DBContext
{
    public partial class Employee : ISelectableItem
    {
        public int? EmployeeId { get; set; }

        [NotMapped]
        public string ItemKey
        {

            get
            {
                return EmployeeId.HasValue ? $"{EmployeeId.Value:D6}" : String.Empty;
            }
        }

        [AllowFiltering]
        [MaxLength(20, ErrorMessage = "Last name would not be longer 20 characters.")]
        public string? LastName { get; set; }

        [AllowFiltering]
        [MaxLength(10, ErrorMessage = "First name would not be longer 10 characters.")]
        public string? FirstName { get; set; }

        [NotMapped]
        public string ItemValue
        {
            get
            {
                string lastName = LastName ?? "";
                string firstName = FirstName ?? "";
                return String.IsNullOrEmpty(lastName) && String.IsNullOrEmpty(firstName) ? "New employee" : $"{FirstName} {LastName}";
            }
        }

        [AllowFiltering]
        [MaxLength(30, ErrorMessage = "Title would not be longer 30 characters.")]
        public string? Title { get; set; }

        [AllowFiltering]
        [MaxLength(25, ErrorMessage = "Title Of courtesy would not be longer 25 characters.")]
        public string? TitleOfCourtesy { get; set; }

        [AllowFiltering]
        [Range(typeof(DateOnly), "1901-01-01", "2100-01-01", ErrorMessage = "Birth date would fit into last and current centuries.")]
        public DateOnly? BirthDate { get; set; }

        [AllowFiltering]
        [Range(typeof(DateOnly), "1901-01-01", "2100-01-01", ErrorMessage = "Hire date would fit into last and current centuries.")]
        public DateOnly? HireDate { get; set; }

        [MaxLength(60, ErrorMessage = "Address would not be longer 60 characters.")]
        public string? Address { get; set; }

        [MaxLength(15, ErrorMessage = "City would not be longer 15 characters.")]
        public string? City { get; set; }

        [MaxLength(15, ErrorMessage = "Region would not be longer 15 characters.")]
        public string? Region { get; set; }

        [MaxLength(10, ErrorMessage = "Postal code would not be longer 10 characters.")]
        public string? PostalCode { get; set; }

        [MaxLength(15, ErrorMessage = "Country would not be longer 15 characters.")]
        public string? Country { get; set; }

        [MaxLength(24, ErrorMessage = "Phone number would not be longer 24 characters.")]
        public string? HomePhone { get; set; }

        public string? Extension { get; set; }

        public byte[]? Photo { get; set; }

        /// <summary>
        /// This property may be used instead of <code>Photo</code>. It will allow you reading file on server (inside <code>HandleSubmit</code> procedure).
        /// You can use <code>@bind-Value="Model!.PhotoFile"</code> but image will be updated after form post.
        /// </summary>
        //[NotMapped]
        //public IFormFile? PhotoFile { get; set; }

        [NotMapped]
        public string PhotoUrl
        {
            get
            {
                if (Photo != null)
                {
                    var b64String = Convert.ToBase64String(this.Photo);
                    return $"data:image/jpg;base64,{b64String}";
                }
                else
                {
                    return "/Images/anonymous.png";
                }
            }
        }

        [NotMapped]
        public IBrowserFile? PhotoFile { get; set; }

        public string? Notes { get; set; }

        public int? ReportsTo { get; set; }

        [NotMapped]
        public string? ReportsToStr
        {
            get
            {
                return ReportsTo.HasValue ? $"{ReportsTo.Value:D6}" : String.Empty;
            }
            set 
            {
                if (String.IsNullOrEmpty(value))
                {
                    ReportsTo = null;
                }
                else
                {
                    ReportsTo = Convert.ToInt32(value);
                }
            }
        }

        public string? PhotoPath { get; set; }

        public byte IsDeleted { get; set; }

        public int Version { get; set; }

        public bool IgnoreConcurency { get; set; } = false;
        public bool RememberRegion { get; set; } = false;


        public virtual ICollection<Employee> InverseReportsToNavigation { get; set; } = new List<Employee>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual Employee? ReportsToNavigation { get; set; }

        /// <summary>
        /// M-M relation to Territory object
        /// </summary>
        public virtual ICollection<Territory> Territories { get; set; } = new List<Territory>();

        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity
                    .ToTable("Employees")
                    .HasKey(e => e.EmployeeId).HasName("PK_Employees");

                entity.Ignore(t => t.IgnoreConcurency);
                entity.Ignore(t => t.RememberRegion);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID").HasColumnType("INTEGER").ValueGeneratedOnAdd();
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(20).HasColumnType("TEXT");
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(10).HasColumnType("TEXT");
                entity.Property(e => e.Title).HasMaxLength(30).HasColumnType("TEXT");
                entity.Property(e => e.TitleOfCourtesy).HasMaxLength(25).HasColumnType("TEXT");
                entity.Property(e => e.BirthDate).HasColumnType("DATE");
                entity.Property(e => e.HireDate).HasColumnType("DATE");
                entity.Property(e => e.Address).HasMaxLength(60).HasColumnType("TEXT");
                entity.Property(e => e.City).HasMaxLength(15).HasColumnType("TEXT");
                entity.Property(e => e.Region).HasMaxLength(15).HasColumnType("TEXT");
                entity.Property(e => e.PostalCode).HasMaxLength(10).HasColumnType("TEXT");
                entity.Property(e => e.Country).HasMaxLength(15).HasColumnType("TEXT");
                entity.Property(e => e.HomePhone).HasMaxLength(24).HasColumnType("TEXT");
                entity.Property(e => e.Extension).HasMaxLength(4).HasColumnType("TEXT");
                entity.Property(e => e.Photo).HasColumnType("BLOB");
                entity.Property(e => e.Notes).HasColumnType("TEXT");
                entity.Property(e => e.ReportsTo).HasColumnType("INTEGER");
                entity.Property(e => e.PhotoPath).HasMaxLength(255).HasColumnType("TEXT");

                entity.Property(e => e.IsDeleted).HasColumnType("INTEGER").HasDefaultValue(0);
                entity.Property(e => e.Version).HasColumnType("INTEGER").HasDefaultValue(0).IsRowVersion();

                entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation).HasForeignKey(d => d.ReportsTo);

                entity.HasMany(d => d.Territories).WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeTerritory",
                        r => r.HasOne<Territory>().WithMany()
                            .HasForeignKey("TerritoryId")
                            .OnDelete(DeleteBehavior.ClientSetNull),
                        l => l.HasOne<Employee>().WithMany()
                            .HasForeignKey("EmployeeId")
                            .OnDelete(DeleteBehavior.ClientSetNull),
                        j =>
                        {
                            j.HasKey("EmployeeId", "TerritoryId");
                            j.ToTable("EmployeeTerritories");
                            j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");
                            j.IndexerProperty<string>("TerritoryId").HasColumnName("TerritoryID");
                        });
            });
        }
    }
}
