using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using ODataV4SampleUsingHttp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ODataV4SampleUsingHttp.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.EnableDependencyInjection();

            var builder = new ODataConventionModelBuilder();

            builder.Namespace = "Default";
            builder.ContainerName = "DefaultContainer";

            var products = builder.EntitySet<Product>("Products");

            var function = products.EntityType.Collection.Function("GetExpensiveProducts");

            function.Parameter<decimal>("minPrice");
            function.ReturnsCollectionFromEntitySet<Product>("Products");

            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            config.MapODataServiceRoute(
               routeName: "ODataRoute",
               routePrefix: "odata",
               model: builder.GetEdmModel());

            config.EnsureInitialized();

        }
    }
}
