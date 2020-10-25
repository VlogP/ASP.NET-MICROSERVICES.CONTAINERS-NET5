using ProductMicroservice.DAL.Models;
using ProductMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMicroservice.DAL.Repositories.Classes
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ProductDBContext context) : base(context)
        {

        }

    }
}
