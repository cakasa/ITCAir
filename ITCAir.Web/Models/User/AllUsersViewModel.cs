using ITCAir.Web.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ITCAir.Web.Models.User
{
    public class AllUsersViewModel
    {
        public PagerViewModel Pager { get; set; }
        public ICollection<UserInfoViewModel> AllUsers { get; set; }
    }
}
