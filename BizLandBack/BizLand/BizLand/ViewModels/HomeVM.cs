using BizLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizLand.ViewModels
{
    public class HomeVM
    {
        public List<TeamMember> teamMembers { get; set; }
        public List<Features> features { get; set; }
    }
}
