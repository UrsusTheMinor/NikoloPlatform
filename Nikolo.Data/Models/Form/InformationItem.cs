namespace Nikolo.Data.Models.Form;

public abstract class InformationItem
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
}