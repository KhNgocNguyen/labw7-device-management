/****************/  
//Mã sinh viên: 202418955 
//Họ tên: Nguyễn Khánh Ngọc
/****************/  
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceManagement;

public class InMemoryRepository<T> : IRepository<T> where T : IIdentifiable
{
    private readonly List<T> _items = new List<T>();

    public void Add(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (_items.Any(i => i.Id == item.Id))
        {
            throw new DuplicateIdException(item.Id);
        }
        _items.Add(item);
    }

    public bool Remove(string id)
    {
        var item = FindById(id);
        if (item != null)
        {
            return _items.Remove(item);
        }
        return false;
    }
    public T? FindById(string id) => _items.FirstOrDefault(i => i.Id == id);

    public IReadOnlyList<T> GetAll() => _items.AsReadOnly();

    public IReadOnlyList<T> FindAll(Predicate<T> condition) 
        => _items.Where(i => condition(i)).ToList().AsReadOnly();
}