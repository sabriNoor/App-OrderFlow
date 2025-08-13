using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.Data;
using App.MVC.Entities;
using App.MVC.Repositories.Interfaces;

namespace App.MVC.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
        }
    }
}