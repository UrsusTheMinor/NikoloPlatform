namespace Nikolo.Data.Models;

public class InformationGroup
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ElementsPerRow { get; set; } = 1;
    public List<InformationType>? InformationTypes { get; set; }
}