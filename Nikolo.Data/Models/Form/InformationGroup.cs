namespace Nikolo.Data.Models.Form;


public class InformationGroup : InformationItem
{
    public int ElementsPerRow { get; set; } = 1;
    public List<InformationType>? InformationTypes { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime ModifiedOn { get; set; } = DateTime.Now;
}