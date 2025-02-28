using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Sharprompt.Enums;

namespace Sharprompt.Example.Models;

public class MyFormModel
{
    [Display(Name = "Id")]
    [BindIgnore]
    public int Id { get; set; }

    public string ReadOnly { get; } = null!;

    [Display(Name = "What's your name?", Prompt = "Required", Order = 1)]
    [Required]
    [SkillLevel(SkillLevel.None)]
    [DefaultValueTabBehaviour(DefaultValueTabBehaviour.TabToSelect)]
    public string Name { get; set; } = null!;

    [Display(Name = "Type new password", Order = 2)]
    [DataType(DataType.Password)]
    [Required]
    [MinLength(2)]
    [SkillLevel(SkillLevel.Advanced)]
    public string Password { get; set; } = null!;

    [Display(Name = "Select enum value", Order = 3)]
    [SkillLevel(SkillLevel.Expert)]
    public MyEnum? MyEnum { get; set; }

    [Display(Name = "Select enum values", Order = 4)]
    [SkillLevel(SkillLevel.Expert)]
    public IEnumerable<MyEnum> MyEnums { get; set; } = null!;

    [Display(Name = "Please add item(s)", Order = 5)]
    [SkillLevel(SkillLevel.Advanced)]
    public IEnumerable<string> Lists { get; set; } = null!;

    [Display(Name = "Are you ready?", Order = 10)]
    [SkillLevel(SkillLevel.Expert)]
    public bool? Ready { get; set; }
}
