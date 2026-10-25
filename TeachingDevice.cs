/****************/  
//Mã sinh viên: 202418955 
//Họ tên: Nguyễn Khánh Ngọc
/****************/  
using System;

namespace DeviceManagement;

public abstract class TeachingDevice : IIdentifiable
{
    public string Id { get; }
    public string Name { get; }
    public string StorageLocation { get; }
    public DeviceStatus Status { get; protected set; }

    protected TeachingDevice(string id, string name, string storageLocation)
    {
        Id = id;
        Name = name;
        StorageLocation = storageLocation;
        Status = DeviceStatus.Available; 
    }

    public abstract bool IsReadyForUse();
    public abstract double GetDefaultPriorityScore();

    public void Borrow()
    {
        if (!IsReadyForUse())
        {
            throw new DeviceNotReadyException(Id, Status);
        }
        Status = DeviceStatus.Borrowed;
    }

    public void Return()
    {
        if (Status != DeviceStatus.Borrowed)
        {
            throw new InvalidOperationException("Cannot return a device that is not borrowed.");
        }
        Status = DeviceStatus.Available;
    }
}