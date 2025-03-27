using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikolo.Data.Models;

public class SkillEmployee
{
    public int SkillId { get; set; }
    public int EmployeeId { get; set; }

    public required Skill Skill { get; set; }
    public required Employee Employee { get; set; }
}