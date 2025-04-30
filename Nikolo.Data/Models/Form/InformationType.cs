namespace Nikolo.Data.Models.Form;

public class InformationType : InformationItem
{
    public string FieldType { get; set; } = string.Empty;
    public InformationGroup? Group { get; set; } = null;
    public int Width { get; set; } = -1;
    public string Placeholder { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime ModifiedOn { get; set; } = DateTime.Now;
    public DateTime? DeletedOn { get; set; }
}