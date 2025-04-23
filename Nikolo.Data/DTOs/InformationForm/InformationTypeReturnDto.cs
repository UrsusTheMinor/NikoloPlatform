namespace Nikolo.Data.DTOs.InformationForm;

public class InformationTypeReturnDto
{
    public int Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public int Index { get; set; }
    public int Width { get; set; }
    public string Placeholder { get; set; } = string.Empty;
}