﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class EditNameViewModel
    {
        [Required]
        public string ScreenName { get; set; }
    }
}