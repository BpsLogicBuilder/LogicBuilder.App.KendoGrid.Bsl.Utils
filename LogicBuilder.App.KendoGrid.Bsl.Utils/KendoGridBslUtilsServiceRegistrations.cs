using LogicBuilder.App.KendoGrid.Bsl.Utils;
using LogicBuilder.App.KendoGrid.Bsl.Utils.Interfaces;

#pragma warning disable IDE0130 //Microsoft recommended namespace for service registrations
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130
{
    public static class KendoGridBslUtilsServiceRegistrations
    {
        public static IServiceCollection AddKendoGridBslUtilsServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IRequestHelper, RequestHelper>();
        }
    }
}
