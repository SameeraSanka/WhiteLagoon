using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        //this a re the properties of villa class
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Price per Night")]
        [Range(10,1000)]
        public double Price { get; set; }
        public int Sqft { get; set; }

        [NotMapped] // meka use krama db ekata data ynne na
        public IFormFile? Image { get; set; } // IformFie use krama thma file upload button eka
                                              // pennanne. View eke asp-for ekata Image kiyna
                                              // eka damma hri ita passe form tag eke enctype
                                              // eka danna one. naththa file upload wenne na.
        [Range(1, 10)]
        public int Occupancy { get; set; }
        [Display(Name="Image Url")]
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
