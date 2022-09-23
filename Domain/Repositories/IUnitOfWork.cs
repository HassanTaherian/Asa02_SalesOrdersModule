﻿using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        int SaveChanges();
    }
}
