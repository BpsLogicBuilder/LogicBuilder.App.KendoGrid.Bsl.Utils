using LogicBuilder.EntityFrameworkCore.Crud.DataStores;

namespace LogicBuilder.App.KendoGrid.Bsl.Utils.Tests.Data.Stores
{
    public class DataClassesStore(DataClassesContext context) : StoreBase(context), IDataClassesStore
    {
    }
}
