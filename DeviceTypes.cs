/****************/  
//Mã sinh viên: 202418955 
//Họ tên: Nguyễn Khánh Ngọc
/****************/ 
using System.Collections.Generic;

namespace DeviceManagement;

public class ProjectorRemote : TeachingDevice
{
    public int BatteryLevel { get; }

    public ProjectorRemote(string id, string name, string location, int battery) 
        : base(id, name, location)
    {
        BatteryLevel = battery;
    }

    public override bool IsReadyForUse() => Status == DeviceStatus.Available && BatteryLevel >= 20;
    
    public override double GetDefaultPriorityScore() => BatteryLevel;
}

public class AirConditionerRemote : TeachingDevice
{
    private readonly HashSet<string> _compatibleRooms;
    public IReadOnlySet<string> CompatibleRooms => _compatibleRooms;

    public AirConditionerRemote(string id, string name, string location, HashSet<string> rooms) 
        : base(id, name, location)
    {
        _compatibleRooms = rooms;
    }

    public bool IsCompatibleWith(string roomId) => _compatibleRooms.Contains(roomId);
    
    public override bool IsReadyForUse() => Status == DeviceStatus.Available;
    
    public override double GetDefaultPriorityScore() => 100;
}

public class WirelessMicrophone : TeachingDevice
{
    public int BatteryLevel { get; }
    public int SignalQuality { get; }

    public WirelessMicrophone(string id, string name, string location, int battery, int signal) 
        : base(id, name, location)
    {
        BatteryLevel = battery;
        SignalQuality = signal;
    }

    public override bool IsReadyForUse() => Status == DeviceStatus.Available && (BatteryLevel >= 30 || SignalQuality >= 40);
    
    public override double GetDefaultPriorityScore() => (BatteryLevel + SignalQuality) / 2.0;
}