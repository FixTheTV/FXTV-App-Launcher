# Giao thức Mạng Hệ thống FXTVGame (Dạng Binary Thô)

## 1. Cấu trúc khung gói tin tổng quát (Packet Header)
Mọi gói tin truyền qua mạng bắt buộc phải bắt đầu bằng 6 byte Header cố định:

- [0 -> 3] (4 bytes): **Length** (Int32) - Tổng độ dài toàn bộ gói tin.
- [4 -> 5] (2 bytes): **Opcode** (Int16) - Mã định danh loại lệnh.

---

## 2. Chi tiết cấu trúc Payload theo từng Opcode


### Opcode 1000: Client gui login request (Client -> Backend)

- **Cấu trúc chi tiết:**
	+ [6] (1 byte): **User Length** - Độ dài username (max 255 ký tự).
	+ [7 -> ...] (byte[]): **Username** - Mảng byte chứa tên tk.
	+ [... -> ... + 1] (byte) : **Password Length** - Độ dài password. 
	+ [... -> ...] (byte[]) : **Password** - Mảng byte chứa password.

### Opcode 1001: Client gui register request (Client -> Backend)

- **Cấu trúc chi tiết:**
	+ [6] (1 byte): **User Length** - Độ dài username (max 255 ký tự).
	+ [7 -> ...] (byte[]): **Username** - Mảng byte chứa tên tk.
	+ [... -> ... + 1] (1 byte) : **Password Length** - Độ dài password. 
	+ [... -> ...] (byte[]) : **Password** - Mảng byte chứa password.

### Opcode 1002: Client gui message (Client -> Backend)

- **Cấu trúc chi tiết:**
	+ [6->7] ( 2 bytes) : **Message Length** - Độ dài password. 
	+ [8-> ...] (byte[]) : **Message** - Mảng byte chứa password.