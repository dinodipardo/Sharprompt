using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Sharprompt.Forms;
using Sharprompt.Internal;

namespace Sharprompt;

public static partial class Enums
{
    //[Flags]
    //public enum SkillLevel
    //{
    //    [Display(Name = "None")]
    //    None = 0,
    //    [Display(Name = "Beginner")]
    //    Beginner = 1,
    //    [Display(Name = "Advanced")]
    //    Advanced = 1 << 1, // 2
    //    [Display(Name = "Expert")]
    //    Expert = 1 << 2, // 4
    //    [Display(Name = "Developer")]
    //    Developer = 1 << 3, // 8
    //    [Display(Name = "All")]
    //    All = Beginner | Advanced | Expert | Developer
    //}

    public enum SkillLevel
    {
        [Display(Name = "Beginner")]
        Beginner = 1,
        [Display(Name = "Advanced")]
        Advanced = 2,
        [Display(Name = "Expert")]
        Expert = 3,
        [Display(Name = "Developer")]
        Developer = 4,
        [Display(Name = "None")]
        None = int.MaxValue,
    }
}
