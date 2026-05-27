using ClassLibraryDatabase.CustomFilter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassLibraryDatabase.DB_Context.Models
{
    public partial class Customer
    {
        [Required]
        [MaxLength(5, ErrorMessage = "Customer ID must be unique and not exceed 5 characters.")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [MaxLength(40, ErrorMessage = "Company name must not exceed 40 characters.")]
        [AllowFiltering]
        public string CompanyName { get; set; } = String.Empty;

        [MaxLength(30, ErrorMessage = "Contact name must not exceed 30 characters.")]
        [AllowFiltering]
        public string? ContactName { get; set; }

        [MaxLength(30, ErrorMessage = "Contact title must not exceed 30 characters.")]
        [AllowFiltering]
        public string? ContactTitle { get; set; }

        [MaxLength(60, ErrorMessage = "Address must not exceed 60 characters.")]
        [AllowFiltering]
        public string? Address { get; set; }

        [MaxLength(15, ErrorMessage = "City name must not exceed 15 characters.")]
        [AllowFiltering]
        public string? City { get; set; }

        public int RegionId { get; set; }

        [MaxLength(15, ErrorMessage = "Region name must not exceed 15 characters.")]
        [AllowFiltering]
        public string? Region { get; set; }

        [MaxLength(10, ErrorMessage = "Postal code must not exceed 10 characters.")]
        [AllowFiltering]
        public string? PostalCode { get; set; }

        [MaxLength(15, ErrorMessage = "Country name must not exceed 15 characters.")]
        [AllowFiltering]
        public string? Country { get; set; }

        [MaxLength(24, ErrorMessage = "Phone number must not exceed 24 characters.")]
        public string? Phone { get; set; }

        [MaxLength(24, ErrorMessage = "Fax number must not exceed 24 characters.")]
        public string? Fax { get; set; }

        public byte IsDeleted { get; set; }

        public int Version { get; set; }

        public bool IgnoreConcurency { get; set; } = false;
        public bool RememberRegion { get; set; } = false;
    }
}
