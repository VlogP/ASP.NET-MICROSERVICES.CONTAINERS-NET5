using ProductDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogicLayer.Services.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAll();

        void Add(Product product);
    }
}
