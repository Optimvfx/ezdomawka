﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Admin
{
    public class ThemeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
