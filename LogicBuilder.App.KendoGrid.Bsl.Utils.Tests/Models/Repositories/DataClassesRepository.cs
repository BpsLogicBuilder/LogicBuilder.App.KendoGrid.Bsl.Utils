using AutoMapper;
using LogicBuilder.App.KendoGrid.Bsl.Utils.Tests.Data.Stores;
using LogicBuilder.EntityFrameworkCore.Repositories;

namespace LogicBuilder.App.KendoGrid.Bsl.Utils.Tests.Models.Repositories
{
    public class DataClassesRepository(IDataClassesStore store, IMapper mapper) : ContextRepositoryBase(store, mapper), IDataClassesRepository
    {
    }
}
