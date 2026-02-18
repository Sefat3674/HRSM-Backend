# HRSM – User Management API

This module of **HRSM (Human Resource & Staff Management System)** handles **user roles, authentication, profile management, and attendance tracking**. It provides RESTful APIs for Admin and Staff to manage users securely and efficiently.

---

## Table of Contents
- [Overview](#overview)  
- [Database Design](#database-design)  
- [API Endpoints](#api-endpoints)  
- [Request & Response Examples](#request--response-examples)  
- [Technologies Used](#technologies-used)  
- [Setup Instructions](#setup-instructions)  

---

## Overview
The **User Management API** enables:
- **Login/Logout** for Admin and Staff  
- **User creation, update, and retrieval**  
- **Profile and role management**  
- **Attendance tracking** per user  
- **Role-based access control**

---

## Database Design
The API uses four tables:

### 1. Roles
- Stores user roles and permissions  
- Columns:
  - `RoleId` (PK)
  - `RoleName` (Unique)
- Example Roles: Admin, Staff, Customer

### 2. Users
- Stores login credentials and role association  
- Columns:
  - `UserId` (PK)
  - `UserName` (Unique)
  - `PasswordHash`
  - `RoleId` (FK → Roles)
  - `IsActive` (Boolean)
  - `CreatedAt`

### 3. UserProfile
- Stores personal information separate from login  
- Columns:
  - `ProfileId` (PK)
  - `UserId` (FK → Users, Unique)
  - `FullName`
  - `Email` (Unique)
  - `Phone`

### 4. Attendance
- Tracks staff attendance  
- Columns:
  - `AttendanceId` (PK)
  - `UserId` (FK → Users)
  - `CheckInTime`
  - `CheckOutTime`
  - `Date`

---

## API Endpoints

| Endpoint | Method | Description | Request Body |
|----------|--------|-------------|--------------|
| `/api/admin/login` | POST | Admin login | `{ "userName": "BB", "password": "Sefat" }` |
| `/api/admin/create-user` | POST | Admin creates a new user | `{ "username": "chef1", "password": "Chef@123", "roleId": 2, "fullName": "Chef John", "email": "chef1@example.com", "phone": "0172222222" }` |
| `/api/admin/logout` | POST | Admin logout | None |
| `/api/staff/login` | POST | Staff login | `{ "userName": "Staff1", "password": "Staff@123" }` |
| `/api/staff/logout` | POST | Staff logout | None |
| `/api/users/update-user` | POST | Update user info | `{ "userId": 2, "fullName": "Chef Johnny", "email": "chefjohn@example.com", "phone": "0172223344" }` |
| `/api/users/{id}` | GET | Get user by ID | None |
| `/api/users` | GET | Get all users | None |
| `/api/roles` | GET | Get all roles | None |
| `/api/profiles` | GET | Get all user profiles | None |
| `/api/attendance/by-user/{userId}` | GET | Get attendance for a specific user | None |

---
---

## Postman Workflow

1. **Create a new Collection**: `HRSM User Management API`  
2. **Set Environment Variables**:
   - `{{baseUrl}}` → API URL
   - `{{adminToken}}`, `{{staffToken}}`, `{{userId}}`, `{{roleId}}`

### Steps:

#### **Step 1: Admin Login**
- **POST** `{{baseUrl}}/api/admin/login`
- **Body (JSON)**:
```json
{
  "userName": "BB",
  "password": "Sefat"
}
Postman Test Script (Save JWT Token):
pm.test("Admin Login successful", function () {
    pm.response.to.have.status(200);
    var jsonData = pm.response.json();
    pm.environment.set("adminToken", jsonData.token);
});
```
### **Step 2: Create User**
- **Endpoint:** `POST {{baseUrl}}/api/admin/create-user`
- **Description:** Admin creates a new user.
- **Headers:**
```http
Authorization: Bearer {{adminToken}}
Content-Type: application/json
- **Body (JSON)**:
```json
{
  "username": "chef1",
  "password": "Chef@123",
  "roleId": 2,
  "fullName": "Chef John",
  "email": "chef1@example.com",
  "phone": "0172222222"
}

Postman Test Script (Save Created User ID):
pm.test("User Created", function () {
    pm.response.to.have.status(200);
    var jsonData = pm.response.json();
    pm.environment.set("userId", jsonData.userId);
});




