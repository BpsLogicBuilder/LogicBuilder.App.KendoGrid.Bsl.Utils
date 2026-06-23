using Kendo.Mvc.UI;
using LogicBuilder.App.KendoGrid.Bsl.Business.Requests;
using LogicBuilder.Data;
using LogicBuilder.Domain;
using System.Threading.Tasks;

namespace LogicBuilder.App.KendoGrid.Bsl.Utils.Interfaces
{
    public interface IRequestHelper
    {
        Task<DataSourceResult> GetData(KendoGridDataRequest request);
        Task<DataSourceResult> GetData<TModel, TData>(KendoGridDataRequest request) where TModel : BaseModel where TData : BaseData;
    }
}
