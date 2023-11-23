using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        
        public int Id { get; set; }

        [Display(Name="名稱")]
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [DisplayName("描述")]
        [Required]
        public string? Description { get; set; }

        [Range(10,10000)]
        public Double Price { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ValidateNever]
        public IEnumerable<Amenity> VillaAmenity { get; set; }

        [NotMapped]
        public bool IsAvailable { get; set; } = true;

    }
}