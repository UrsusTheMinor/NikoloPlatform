namespace Nikolo.Data.DTOs.InformationForm.Type;

public class InformationTypeEditDto
{
    public int Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public int? Width { get; set; }
    public string Placeholder { get; set; } = string.Empty;
}