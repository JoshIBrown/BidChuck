﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCWeb.Models
{
    public interface IGenericRepository<T> where T : class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(T entity);
        T Get(int id);
        IQueryable<T> Query();
        void Save();
        bool Exists(int id);
    }
}