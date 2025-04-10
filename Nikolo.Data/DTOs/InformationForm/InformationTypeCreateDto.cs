namespace Nikolo.Data.DTOs.InformationForm;

public class InformationTypeCreateDto
{
    public string TypeName { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public int GroupId { get; set; }
    public int Index { get; set; }
}