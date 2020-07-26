using ProductDataLayer.Models;
using ProductDataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductDataLayer.Repositories.Classes
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ProductDBContext context) : base(context)
        {

        }

    }
}
