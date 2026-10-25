/****************/  
//Mã sinh viên: 202418955 
//Họ tên: Nguyễn Khánh Ngọc
/****************/ 
using System;
using System.Linq;

namespace DeviceManagement;

public class AllocationService<T> where T : TeachingDevice
{
    private readonly IRepository<T> _repository;
    private ISelectionStrategy<T> _strategy;

    public AllocationService(IRepository<T> repository, ISelectionStrategy<T> strategy)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public void ChangeStrategy(ISelectionStrategy<T> newStrategy)
    {
        _strategy = newStrategy ?? throw new ArgumentNullException(nameof(newStrategy));
    }

    public T Allocate(Predicate<T>? additionalCondition = null)
    {
        var allDevices = _repository.GetAll();
        
        var readyDevices = allDevices.Where(d => d.IsReadyForUse());
        
        if (additionalCondition != null)
        {
            readyDevices = readyDevices.Where(d => additionalCondition(d));
        }

        var candidates = readyDevices.ToList();
        
        var selectedDevice = _strategy.Select(candidates);
        
        selectedDevice.Borrow();
        
        return selectedDevice;
    }
}