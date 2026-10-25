/****************/  
//Mã sinh viên: 202418955 
//Họ tên: Nguyễn Khánh Ngọc
/****************/  
using System;
using System.Collections.Generic;

namespace DeviceManagement;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== HỆ THỐNG QUẢN LÝ THIẾT BỊ DẠY HỌC ===");

        // Khởi tạo Repositories
        var projRepo = new InMemoryRepository<ProjectorRemote>();
        var acRepo = new InMemoryRepository<AirConditionerRemote>();
        var micRepo = new InMemoryRepository<WirelessMicrophone>();

        // Chuẩn bị dữ liệu mẫu
        var proj1 = new ProjectorRemote("PRJ01", "Remote Máy chiếu 1", "Phòng Giáo viên", 15); // Pin thấp
        var proj2 = new ProjectorRemote("PRJ02", "Remote Máy chiếu 2", "Phòng Giáo viên", 90);
        var proj3 = new ProjectorRemote("PRJ03", "Remote Máy chiếu 3", "Kho 1", 50);

        var ac1 = new AirConditionerRemote("AC01", "Remote Điều hòa 1", "Kho 1", new HashSet<string> { "A203", "A204" });
        var ac2 = new AirConditionerRemote("AC02", "Remote Điều hòa 2", "Kho 2", new HashSet<string> { "B101" });

        var mic1 = new WirelessMicrophone("MIC01", "Micro 1", "Kho 1", 20, 50); // Pin thấp
        var mic2 = new WirelessMicrophone("MIC02", "Micro 2", "Kho 2", 80, 20); // Tín hiệu thấp
        var mic3 = new WirelessMicrophone("MIC03", "Micro 3", "Kho 1", 100, 100);

        Console.WriteLine("\n--- TEST 1: Thêm thiết bị vào repository ---");
        projRepo.Add(proj1); projRepo.Add(proj2); projRepo.Add(proj3);
        acRepo.Add(ac1); acRepo.Add(ac2);
        micRepo.Add(mic1); micRepo.Add(mic2); micRepo.Add(mic3);
        Console.WriteLine($"Đã thêm {projRepo.GetAll().Count} Projector, {acRepo.GetAll().Count} AC, {micRepo.GetAll().Count} Mic.");

        Console.WriteLine("\n--- TEST 2: Thử thêm hai thiết bị trùng mã ---");
        try
        {
            projRepo.Add(new ProjectorRemote("PRJ01", "Duplicate", "N/A", 100));
        }
        catch (DuplicateIdException ex)
        {
            Console.WriteLine($"Bắt lỗi thành công: {ex.Message}");
        }

        Console.WriteLine("\n--- TEST 3: Tìm thiết bị theo mã ---");
        var found = projRepo.FindById("PRJ02");
        Console.WriteLine(found != null ? $"Tìm thấy: {found.Name}" : "Không tìm thấy");

        Console.WriteLine("\n--- TEST 4: Lọc các thiết bị sẵn sàng ---");
        var readyMics = micRepo.FindAll(m => m.IsReadyForUse());
        Console.WriteLine($"Số mic sẵn sàng (Cẩn thận Bug ở điều kiện mic!): {readyMics.Count}"); 

        Console.WriteLine("\n--- TEST 5: Chọn điều khiển máy chiếu có điểm cao nhất ---");
        var highestScoreStrategy = new HighestPriorityStrategy<ProjectorRemote>(p => p.GetDefaultPriorityScore());
        var projService = new AllocationService<ProjectorRemote>(projRepo, highestScoreStrategy);
        var bestProj = projService.Allocate();
        Console.WriteLine($"Máy chiếu được chọn: {bestProj.Id} - Pin: {bestProj.BatteryLevel}% (Kiểm tra xem có đúng là cao nhất chưa do BUG 3)");

        Console.WriteLine("\n--- TEST 6: Chọn điều khiển điều hòa tương thích với một phòng ---");
        var firstAcStrategy = new FirstAvailableStrategy<AirConditionerRemote>();
        var acService = new AllocationService<AirConditionerRemote>(acRepo, firstAcStrategy);
        var suitableAc = acService.Allocate(device => device.IsCompatibleWith("A203"));
        Console.WriteLine($"Điều hòa cấp cho phòng A203: {suitableAc.Id}");

        Console.WriteLine("\n--- TEST 7: Chọn micro đầu tiên sẵn sàng ---");
        var firstMicStrategy = new FirstAvailableStrategy<WirelessMicrophone>();
        var micService = new AllocationService<WirelessMicrophone>(micRepo, firstMicStrategy);
        var firstMic = micService.Allocate();
        Console.WriteLine($"Mic được chọn: {firstMic.Id}");

        Console.WriteLine("\n--- TEST 8: Đổi chiến lược trong thời gian chạy và quan sát kết quả ---");
        var highestMicStrategy = new HighestPriorityStrategy<WirelessMicrophone>(m => m.GetDefaultPriorityScore());
        micService.ChangeStrategy(highestMicStrategy);
        var nextMic = micService.Allocate();
        Console.WriteLine($"Mic được chọn sau khi đổi sang HighestPriority: {nextMic.Id}");

        Console.WriteLine("\n--- TEST 9: Thử cấp phát khi không có ứng viên phù hợp ---");
        try
        {
            acService.Allocate(device => device.IsCompatibleWith("C999"));
        }
        catch (NoSuitableItemException)
        {
            Console.WriteLine("Bắt lỗi thành công: NoSuitableItemException (Phòng C999 không có remote tương thích).");
        }

        Console.WriteLine("\n--- TEST 10: Chứng minh thiết bị được chọn chuyển sang Borrowed ---");
        Console.WriteLine($"Trạng thái hiện tại của {firstMic.Id} là: {firstMic.Status} (Chú ý BUG 1 ở đây!)");

        Console.WriteLine("\n--- TEST 11: Chứng minh repository tổng quát dùng được cho nhiều loại ---");
        Console.WriteLine($"Repository của AC ({acRepo.GetType().Name}) và Projector ({projRepo.GetType().Name}) sử dụng chung logic generic, không lặp code.");
    }
}
