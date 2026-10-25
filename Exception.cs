/****************/  
//Mã sinh viên: 202418955 
//Họ tên: Nguyễn Khánh Ngọc
/****************/  
using System;

namespace DeviceManagement;

public class DuplicateIdException : Exception
{
    public string DuplicateId { get; }
    public DuplicateIdException(string id) : base($"Device with ID '{id}' already exists.")
    {
        DuplicateId = id;
    }
}

public class NoSuitableItemException : Exception
{
    public NoSuitableItemException() : base("No suitable item found for the requested criteria.") { }
}

public class DeviceNotReadyException : Exception
{
    public string DeviceId { get; }
    public DeviceStatus CurrentStatus { get; }
    
    public DeviceNotReadyException(string id, DeviceStatus status) 
        : base($"Device '{id}' is not ready for use. Current status: {status}")
    {
        DeviceId = id;
        CurrentStatus = status;
    }
}