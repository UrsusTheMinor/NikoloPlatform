namespace Nikolo.Data.DTOs.InformationForm;

public class MoveDto
{
    public int FromIndex { get; set; }
    public int ToIndex { get; set; }
    public int? TargetGroupId { get; set; } // group to move the item to
    public int? CurrentGroupId { get; set; }
}