/****************/  
//Mã sinh viên: 202418955 
//Họ tên: Nguyễn Khánh Ngọc
/****************/  
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceManagement;

public class HighestPriorityStrategy<T> : ISelectionStrategy<T>
{
    private readonly Func<T, double> _scoreSelector;

    public HighestPriorityStrategy(Func<T, double> scoreSelector)
    {
        _scoreSelector = scoreSelector ?? throw new ArgumentNullException(nameof(scoreSelector));
    }

    public T Select(IReadOnlyList<T> candidates)
    {
        if (candidates == null || !candidates.Any())
            throw new NoSuitableItemException();
        return candidates.OrderBy(_scoreSelector).First();
    }
}

public class FirstAvailableStrategy<T> : ISelectionStrategy<T>
{
    public T Select(IReadOnlyList<T> candidates)
    {
        if (candidates == null || !candidates.Any())
            throw new NoSuitableItemException();

        return candidates.First();
    }
}