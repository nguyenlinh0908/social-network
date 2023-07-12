lưu ý nhóm dùng dotnet v7 
<br>
cài đặt đầy đủ các gói trong file social_network.csproj
<br>
sử dụng sql server, thay đổi ConnectionStrings.Default trong file appsettings.json thành kết nối của các nhân.

#lưu ý khi thêm mới và cần update model chạy lệnh dưới
<br>
! cài dotnet-ef nếu chưa có  dotnet tool install --global dotnet-ef 
<br>
dotnet ef migrations add InitialCreate // nếu đã có folder Migrations thì không cần chạy 
<br>
dotnet ef database update