using ODataV4SampleUsingHttp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataV4SampleUsingHttp.Web.Data
{
    public static class ProductRepository
    {
        private static readonly List<Product> _products;

        static ProductRepository()
        {
            _products = new List<Product>();

            for (int i = 1; i <= 50000; i++)
            {
                _products.Add(new Product
                {
                    Id = i,
                    Name = "Product " + i,
                    Price = i * 1.10m
                });
            }
        }

        public static IQueryable<Product> GetAll()
        {
            return _products.AsQueryable();
        }
    }
}