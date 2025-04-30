using Nikolo.Data.DTOs.InformationForm.Type;

namespace Nikolo.Data.DTOs.InformationForm.Group;

public class InformationGroupReturnDto
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ElementsPerRow { get; set; }
    public List<InformationTypeReturnDto>? InformationTypes { get; set; }
}