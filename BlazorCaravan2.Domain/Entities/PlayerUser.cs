using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CaravanDomain.Entities {
    public class PlayerUser : IdentityUser {
        public int Money { get; set; }
    }
}
