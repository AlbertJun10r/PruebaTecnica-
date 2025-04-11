﻿using SistemaVentas.Models;

namespace SistemaVentas.Services.Interfaces
{
    public interface IService<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<T> CreateAsync(T entity);
        public Task UpdateAsync(int id, T entity);
        public Task DeleteAsync(int id);
    }
}
