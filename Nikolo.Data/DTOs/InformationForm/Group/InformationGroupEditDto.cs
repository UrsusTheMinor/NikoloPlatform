namespace Nikolo.Data.DTOs.InformationForm.Group;

public class InformationGroupEditDto
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public int? ElementsPerRow { get; set; }
}