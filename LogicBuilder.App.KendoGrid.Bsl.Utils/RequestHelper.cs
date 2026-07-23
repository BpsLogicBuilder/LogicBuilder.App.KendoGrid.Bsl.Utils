using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using LogicBuilder.App.KendoGrid.Bsl.Business.Requests;
using LogicBuilder.App.KendoGrid.Bsl.Utils.Interfaces;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.Data;
using LogicBuilder.Domain;
using LogicBuilder.EntityFrameworkCore.Repositories;
using LogicBuilder.Expressions.Utils.Expansions;
using LogicBuilder.Kendo.ExpressionExtensions.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace LogicBuilder.App.KendoGrid.Bsl.Utils
{
    public class RequestHelper(IContextRepository contextRepository, IMappingOperations mappingOperations) : IRequestHelper
    {
        private readonly IContextRepository _contextRepository = contextRepository;
        private readonly IMappingOperations _mappingOperations = mappingOperations;

        public Task<DataSourceResult> GetData(KendoGridDataRequest request)
        {
            MethodInfo methodInfo = GetMethodInfo(nameof(GetData)).MakeGenericMethod
            (
                Type.GetType(request.ModelType ?? "") ?? throw new InvalidOperationException($"Model type {request.ModelType} is invalid. Provide a valid asembly qualified type name."),
                Type.GetType(request.DataType ?? "") ?? throw new InvalidOperationException($"Data type {request.DataType} is invalid. Provide a valid asembly qualified type name.")
            );

            return (Task<DataSourceResult>)methodInfo.Invoke(this, [request])!;//Generic GetData never returns null
        }

        public Task<DataSourceResult> GetData<TModel, TData>(KendoGridDataRequest request)
            where TModel : BaseModel
            where TData : BaseData 
            => GetDataSourceResult<TModel, TData>
            (
                CreateDataSourceRequest(request.Options),
                request.SelectExpandDefinition == null ? null : _mappingOperations.MapExpansion(request.SelectExpandDefinition)
            );

        private static DataSourceRequest CreateDataSourceRequest(KendoGridDataSourceRequestOptions? requestOptions)
        {
            var request = new DataSourceRequest();

            if (requestOptions?.Sort != null)
                request.Sorts = DataSourceDescriptorSerializer.Deserialize<SortDescriptor>(requestOptions.Sort);

            request.Page = requestOptions?.Page ?? 0;

            request.PageSize = requestOptions?.PageSize ?? 0;

            if (requestOptions?.Filter != null)
                request.Filters = FilterDescriptorFactory.Create(requestOptions.Filter);

            if (requestOptions?.Group != null)
                request.Groups = DataSourceDescriptorSerializer.Deserialize<GroupDescriptor>(requestOptions.Group);

            if (requestOptions?.Aggregate != null)
                request.Aggregates = DataSourceDescriptorSerializer.Deserialize<AggregateDescriptor>(requestOptions.Aggregate);

            return request;
        }

        private Task<DataSourceResult> GetDataSourceResult<TModel, TData>(DataSourceRequest request, SelectExpandDefinition? selectExpandDefinition = null)
           where TModel : BaseModel
           where TData : BaseData
           => request.Groups != null && request.Groups.Count > 0
               ? GetGroupedDataSourceResult<TModel, TData>(request, selectExpandDefinition)
               : GetUngroupedDataSourceResult<TModel, TData>(request, selectExpandDefinition);

        private async Task<DataSourceResult> GetGroupedDataSourceResult<TModel, TData>(DataSourceRequest request, SelectExpandDefinition? selectExpandDefinition = null)
            where TModel : BaseModel
            where TData : BaseData
        {
            bool getAggregates = request.Aggregates.Count > 0;
            Expression<Func<IQueryable<TModel>, int>> totalExp = QueryableExtensionsEx.CreateTotalExpression<TModel>(request);

            return new DataSourceResult
            {
                Data = await GetData(),
                AggregateResults = getAggregates//The aggregate expression in GetAggregateFunctionsGroup() may return null
                                    ? (await GetAggregateFunctionsGroup())?.GetAggregateResults(request.Aggregates.SelectMany(a => a.Aggregates)) /*getAggregates is true*/
                                    : null,
                Total = await _contextRepository.QueryAsync<TModel, TData, int, int>(totalExp, selectExpandDefinition)
            };

            async Task<IEnumerable> GetData()
            {
                var groupByExpressions = request.CreateGroupedByQueryExpressions<TModel>();
                IQueryable<TModel> pagedQuery = await _contextRepository.QueryAsync<TModel, TData, IQueryable<TModel>, IQueryable<TData>>(groupByExpressions.PagingExpression, selectExpandDefinition);
                return groupByExpressions.GroupByExpression.Compile()(pagedQuery);
            }

            async Task<AggregateFunctionsGroup> GetAggregateFunctionsGroup()
            {
                var aggrewgatexpressions = request.CreateAggregatesQueryExpressions<TModel>();
                IQueryable<TModel> pagedQuery = await _contextRepository.QueryAsync<TModel, TData, IQueryable<TModel>, IQueryable<TData>>(aggrewgatexpressions.QueryableExpression, selectExpandDefinition);
                return aggrewgatexpressions.AggregateExpression.Compile()(pagedQuery);
            }
        }

        private static MethodInfo GetMethodInfo(string methodName)
           => typeof(RequestHelper).GetMethods().Single(m => m.Name == methodName && m.IsGenericMethod);

        private async Task<DataSourceResult> GetUngroupedDataSourceResult<TModel, TData>(DataSourceRequest request, SelectExpandDefinition? selectExpandDefinition = null)
            where TModel : BaseModel
            where TData : BaseData
        {
            bool getAggregates = request.Aggregates.Count > 0;
            Expression<Func<IQueryable<TModel>, int>> totalExp = QueryableExtensionsEx.CreateTotalExpression<TModel>(request);
            Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> ungroupedExp = QueryableExtensionsEx.CreateUngroupedQueryableExpression<TModel>(request);

            return new DataSourceResult
            {
                Data = await _contextRepository.QueryAsync<TModel, TData, IQueryable<TModel>, IQueryable<TData>>(ungroupedExp, selectExpandDefinition),
                AggregateResults = getAggregates//The aggregate expression in GetAggregateFunctionsGroup() may return null
                                    ? (await GetAggregateFunctionsGroup())?.GetAggregateResults(request.Aggregates.SelectMany(a => a.Aggregates)) /*getAggregates is true*/
                                    : null,
                Total = await _contextRepository.QueryAsync<TModel, TData, int, int>(totalExp, selectExpandDefinition)
            };

            async Task<AggregateFunctionsGroup> GetAggregateFunctionsGroup()
            {
                var aggrewgatexpressions = request.CreateAggregatesQueryExpressions<TModel>();
                IQueryable<TModel> pagedQuery = await _contextRepository.QueryAsync<TModel, TData, IQueryable<TModel>, IQueryable<TData>>(aggrewgatexpressions.QueryableExpression, selectExpandDefinition);
                return aggrewgatexpressions.AggregateExpression.Compile()(pagedQuery);
            }
        }
    }
}
