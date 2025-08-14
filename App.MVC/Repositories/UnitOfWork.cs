using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.Repositories;
using App.MVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.MVC.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _dbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task BeginTransactionAsync()
        {
            _transaction=await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if(_transaction is not null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if(_transaction is not null)
                await _transaction.CommitAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}