using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using ODataV4SampleUsingHttp.Web.Data;
using ODataV4SampleUsingHttp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ODataV4SampleUsingHttp.Web.Controllers
{
    [ODataRoutePrefix("Products")]
    public class ProductsController : ODataController
    {
        [EnableQuery(PageSize = 1000)]
        public IQueryable<Product> Get()
        {
            return ProductRepository.GetAll();
        }

        [EnableQuery(PageSize = 100)]
        [HttpGet]
        [ODataRoute("Default.GetExpensiveProducts(minPrice={minPrice})")]
        public IQueryable<Product> GetExpensiveProducts(
           [FromODataUri] decimal minPrice)
        {
            return ProductRepository.GetAll()
                                    .Where(p => p.Price > minPrice);
        }
    }
}