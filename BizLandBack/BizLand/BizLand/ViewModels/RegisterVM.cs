using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BizLand.ViewModels
{
    public class RegisterVM
    {
        [Required, MaxLength(100)]
        public string Name{ get; set; }
        [Required, MaxLength(100)]
        public string Surname { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


    }
}
