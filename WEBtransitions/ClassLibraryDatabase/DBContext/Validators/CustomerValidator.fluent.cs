using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBtransitions.ClassLibraryDatabase.DBContext;

namespace ClassLibraryDatabase.DBContext.Validators
{
    public class CustomerValidator: AbstractValidator<Customer>
    {
        public CustomerValidator() 
        {
            RuleSet("PrimaryKey", () =>
            {
                RuleFor(c => c.CustomerId).NotNull().WithMessage("Customer ID must be defined");
                RuleFor(c => c.CustomerId).NotEmpty().WithMessage("Customer ID would not be empy");
                RuleFor(c => c.CustomerId).MaximumLength(5).WithMessage("Customer ID cannot be longer than 5 characters");
            });
            RuleSet("CommonFields", () =>
            {
                RuleFor(c => c.CompanyName).NotNull().WithMessage("Company name must be defined");
                RuleFor(c => c.CompanyName).NotEmpty().WithMessage("Company name would not be empy");
                RuleFor(c => c.CompanyName).MaximumLength(40).WithMessage("Company name cannot be longer than 40 characters");
                RuleFor(c => c.ContactName).MaximumLength(30).WithMessage("Contact name cannot be longer than 30 characters");
                RuleFor(c => c.ContactTitle).MaximumLength(30).WithMessage("Contact title cannot be longer than 30 characters");
                RuleFor(c => c.Address).MaximumLength(60).WithMessage("Address cannot be longer than 60 characters");
                RuleFor(c => c.City).MaximumLength(15).WithMessage("City cannot be longer than 15 characters");
                RuleFor(c => c.Region).MaximumLength(15).WithMessage("Region cannot be longer than 15 characters");
                RuleFor(c => c.PostalCode).MaximumLength(10).WithMessage("Postal code cannot be longer than 10 characters");
                RuleFor(c => c.Country).MaximumLength(15).WithMessage("Country cannot be longer than 15 characters");
                RuleFor(c => c.Phone).MaximumLength(24).WithMessage("Phone number cannot be longer than 24 characters");
                RuleFor(c => c.Fax).MaximumLength(24).WithMessage("Fax number cannot be longer than 24 characters");
            });
        }
    }
}
