using AutoMapper;
using LogicBuilder.App.KendoGrid.Bsl.Utils.Tests.Data.Stores;
using LogicBuilder.EntityFrameworkCore.Repositories;

namespace LogicBuilder.App.KendoGrid.Bsl.Utils.Tests.Models.Repositories
{
    public class SchoolRepository(ISchoolStore store, IMapper mapper) : ContextRepositoryBase(store, mapper), ISchoolRepository
    {
    }
}
