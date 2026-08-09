# Giao thức Mạng Hệ thống FXTVGame (Dạng Binary Thô)

## 1. Cấu trúc khung gói tin tổng quát (Packet Header)
Mọi gói tin truyền qua mạng bắt buộc phải bắt đầu bằng 6 byte Header cố định:

- [0 -> 3] (4 bytes): **Length** (Int32) - Tổng độ dài toàn bộ gói tin.
- [4 -> 5] (2 bytes): **Opcode** (Int16) - Mã định danh loại lệnh.

---

## 2. Chi tiết cấu trúc Payload theo từng Opcode


### Opcode 1001: Client gui login request (Client -> Backend)

- **Cấu trúc chi tiết:**
	+ [6] (1 byte): **User Length** - Độ dài username (max 255 ký tự).
	+ [7 -> ...] (byte[]): **Username** - Mảng byte chứa tên tk.
	+ [... -> ... + 1] (byte) : **Password Length** - Độ dài password. 
	+ [... -> ...] (byte[]) : **Password** - Mảng byte chứa password.

### Opcode 1002: Client gui register request (Client -> Backend)

- **Cấu trúc chi tiết:**
	+ [6] (1 byte): **User Length** - Độ dài username (max 255 ký tự).
	+ [7 -> ...] (byte[]): **Username** - Mảng byte chứa tên tk.
	+ [... -> ... + 1] (1 byte) : **Password Length** - Độ dài password. 
	+ [... -> ...] (byte[]) : **Password** - Mảng byte chứa password.

### Opcode 2001: Backend gui login result (Backend -> Client)

- **Cấu trúc chi tiết: NEU FAIL **
	+ [6] (1 byte): **Auth result** = 0 - Kết quả.
- **Cấu trúc chi tiết: NEU SUCC **
	+ [6] (1 byte): **Auth result** = 1 - Kết quả
	+ [7] (1 byte): **Username Length** - Độ dài username (max 255 char).
	+ [8 - n] (byte[]): **Username** - Mảng byte chứa tên tk vừa đăng nhập.
	+ [n+1 - n+8] ( 8 byte ): **UserID** - 8 byte chứ userid.


### Opcode 2002: Backend gui register result (Backend -> Client)

- **Cấu trúc chi tiết:**
- **Cấu trúc chi tiết: NEU FAIL **
	+ [6] (1 byte): **Auth result** = 0 - Kết quả.
- **Cấu trúc chi tiết: NEU SUCC **
	+ [6] (1 byte): **Auth result** = 1 - Kết quả
	+ [7] (1 byte): **Username Length** - Độ dài username (max 255 char).
	+ [8 - n] (byte[]): **Username** - Mảng byte chứa tên tk vừa đki.


