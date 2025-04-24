namespace Nikolo.Data.Models;

public class InformationType
{
    public int Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public InformationGroup? Group { get; set; } = null;
    public int Index { get; set; }
    public int Width { get; set; } = -1;
    public string Placeholder { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime ModifiedOn { get; set; } = DateTime.Now;
    public DateTime? DeletedOn { get; set; }
}