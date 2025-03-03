using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Sharprompt.Forms;
using Sharprompt.Internal;

namespace Sharprompt;

public static partial class Enums
{
    public enum SkillLevel
    {
        [Display(Name = "Beginner")]
        Beginner = 1,
        [Display(Name = "Advanced")]
        Advanced = 2,
        [Display(Name = "Expert")]
        Expert = int.MaxValue,
    }
}
