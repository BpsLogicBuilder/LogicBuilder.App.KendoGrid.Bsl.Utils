using Microsoft.EntityFrameworkCore;

namespace LogicBuilder.App.KendoGrid.Bsl.Utils.Tests.Data.Configurations
{
    interface ITableConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}
