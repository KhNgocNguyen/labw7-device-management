/****************/  
//Mã sinh viên: 202418955 
//Họ tên: Nguyễn Khánh Ngọc
/****************/ 
using System;
using System.Collections.Generic;

namespace DeviceManagement;

public interface IIdentifiable
{
    string Id { get; }
}

public interface IRepository<T> where T : IIdentifiable
{
    void Add(T item);
    bool Remove(string id);
    T? FindById(string id);
    IReadOnlyList<T> GetAll();
    IReadOnlyList<T> FindAll(Predicate<T> condition);
}

public interface ISelectionStrategy<T>
{
    T Select(IReadOnlyList<T> candidates);
}