﻿using Library.Models;

namespace Library.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> Get(int id);
        IEnumerable<T> GetAll();
        //IEnumerable<T> GetAllById(int id);//chỉ detail sử dụng
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        //Task DeleteMany(IEnumerable<T> entity);//chỉ detail sử dụng
    }
}
