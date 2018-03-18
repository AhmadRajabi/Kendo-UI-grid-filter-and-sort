using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DNTPersianUtils.Core;

namespace KendoGrid.Web.Models.Entity
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? Date { get; set; }
        public decimal Price { get; set; }
        [NotMapped]
        public string PersianDate => Date.ToShortPersianDateString();
    }
}
