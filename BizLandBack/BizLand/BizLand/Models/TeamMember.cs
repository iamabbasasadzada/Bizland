using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BizLand.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string InstaLink { get; set; }
        [Required]
        public string FacebookLink { get; set; }
        [Required]
        public string TwitterLink { get; set; }
        [Required]
        public string LinkedinLink { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo{ get; set; }



    }
}
