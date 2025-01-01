Dự án Quản lý Nhân sự (MHR)
Đây là một ứng dụng web quản lý nhân sự được phát triển bằng ASP.NET Core (.NET 9) và SQL Server. Ứng dụng cho phép quản lý thông tin nhân viên, chấm công, bảng lương và các chức năng liên quan khác.

Mục lục
Giới thiệu
Cài đặt
Cấu hình
Sử dụng
Công nghệ sử dụng
Đóng góp
Giấy phép
Giới thiệu
Ứng dụng MHR được thiết kế để giúp các doanh nghiệp quản lý hiệu quả thông tin nhân sự, bao gồm:

Quản lý hồ sơ nhân viên
Chấm công và theo dõi thời gian làm việc
Tính toán và quản lý bảng lương
Quản lý các phòng ban và chức vụ
Cài đặt
Yêu cầu hệ thống:

.NET 9 SDK
SQL Server (phiên bản mới nhất)
Visual Studio 2022 hoặc Visual Studio Code

Các cấu hình chính của ứng dụng nằm trong file appsettings.json

Chuỗi kết nối cơ sở dữ liệu:

json
Sao chép mã
"ConnectionStrings": {
  "MHRDatabase": "Server=.;Database=MHRDatabase;Trusted_Connection=True;TrustServerCertificate=True"
}

Công nghệ sử dụng
ASP.NET Core (.NET 9)
Entity Framework Core
SQL Server
Bootstrap 5
Identity Framework cho xác thực và phân quyền
