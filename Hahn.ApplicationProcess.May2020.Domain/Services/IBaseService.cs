using System;
using System.Collections.Generic;
using System.Text;

namespace Hahn.ApplicationProcess.May2020.Domain.Services
{
    public interface IBaseService<T>
    {
        T Find(int Id);
        List<T> GetAll();
        bool Delete(T Item);
    }
}
