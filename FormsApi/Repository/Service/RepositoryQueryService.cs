using System;
using FormsApi.Repository.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace FormsApi.Repository.Service;

public class RepositoryQueryService<T>(IRepositoryQueryHandler<T> repository, string criteria) : IRepositoryCallable
    where T : class
{
    public async Task<object> Invoke()
    {
        IEdmModel model = GetEdmModel();
        HttpRequest request = new DefaultHttpContext().Request;
        request.QueryString = new QueryString("?" + criteria);
        var context = new ODataQueryContext(model, typeof(T), null);
        var options = new ODataQueryOptions<T>(context, request);
        IQueryable<T> queryable = await repository.QueryAsync();
        return options.ApplyTo(queryable).Cast<T>().ToList();
    }

    private IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<T>(typeof(T).Name);
        return builder.GetEdmModel();
    }
}
