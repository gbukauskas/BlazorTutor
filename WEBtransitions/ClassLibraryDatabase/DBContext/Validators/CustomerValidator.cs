using System.ComponentModel.DataAnnotations;

namespace WEBtransitions.ClassLibraryDatabase.DBContext;

public class CustomerMetaData
{
#pragma warning disable CS8618

    [Required]
    [MaxLength(5)]
    public object CustomerId { get; set; }

    [Required]
    [MaxLength(40)]
    public object CompanyName { get; set; }

    [MaxLength(30)]
    public object ContactName { get; set; }

    [MaxLength(30)]
    public object ContactTitle { get; set; }

    [MaxLength(60)]
    public object Address { get; set; }

    [MaxLength(15)]
    public object City { get; set; }

    [MaxLength(15)]
    public object Region { get; set; }

    [MaxLength(10)]
    public object PostalCode { get; set; }

    [MaxLength(15)]
    public object Country { get; set; }

    [MaxLength(24)]
    public object Phone { get; set; }

    [MaxLength(24)]
    public object Fax { get; set; }

    [AllowedValues(0, 1)]
    public object IsDeleted { get; set; }

#pragma warning restore CS8618



    //public int Version { get; set; }

    //public bool IgnoreConcurency { get; set; } = false;
    //public bool RememberRegion { get; set; } = false;

}
