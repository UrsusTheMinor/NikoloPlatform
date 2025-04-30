using Nikolo.Data.Models;
using Nikolo.Data.Models.Form;

namespace Nikolo.Data.Extensions;

public static class InformationTypeExtensions
{
    public static IQueryable<InformationType> NotDeleted(this IQueryable<InformationType> query)
    {
        return query.Where(x => x.DeletedOn == null);
    }
}
