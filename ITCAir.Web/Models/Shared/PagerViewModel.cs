﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Shared
{
    public class PagerViewModel
    {
        public int PageSize {get;set;}
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
