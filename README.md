# 💰 Bütçe Yönetim Sistemi (Console Application)

Bu proje, C# ve **ADO.NET** teknolojileri kullanılarak geliştirilmiş, **MSSQL** veritabanı ile etkileşimli çalışan bir bütçe takip ve yönetim sistemidir. ORM araçları (Entity Framework vb.) kullanılmadan, saf SQL sorguları ile temel CRUD (Create, Read, Update, Delete) işlemlerini gerçekleştirmek amacıyla eğitim projesi olarak hazırlanmıştır.

## 🚀 Özellikler

* **Harcama Ekleme:** Kullanıcıdan alınan verileri veritabanına güvenli bir şekilde (Parameterize edilmiş sorgularla) kaydeder.
* **Harcama Listeleme:** Veritabanındaki kayıtları çeker ve konsol ekranında listeler.
* **Harcama Silme:** ID bazlı silme işlemi yapar ve kullanıcıyı bilgilendirir.
* **Harcama Güncelleme:** Mevcut kayıtların detaylarını değiştirir.
* **SQL Injection Koruması:** `SqlParameter` kullanımı ile güvenlik sağlanmıştır.

## 🛠️ Teknolojiler

* .NET 6.0 / 7.0 (Console App)
* C#
* MSSQL Server
* System.Data.SqlClient (ADO.NET)

## ⚙️ Kurulum ve Çalıştırma

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

### 1. Veritabanı Kurulumu
SQL Server Management Studio (SSMS) üzerinde aşağıdaki scripti çalıştırarak veritabanını ve tabloyu oluşturun:

```sql
CREATE DATABASE ButceDB;
GO
USE ButceDB;

CREATE TABLE Harcamalar (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Baslik NVARCHAR(50),
    Tutar DECIMAL(18,2),
    Tarih DATETIME DEFAULT GETDATE(),
    Kategori NVARCHAR(30)
);
----------------------------------------
Eğer yerel veri merkezinizde çalıştıracaksanız kullanmanız gereken bağlantı kodu-ben başka bir cihazdaki mssql'e bağlandım-:
static string connectionString = "Server=SUNUCU_ADINIZ;Database=ButceDB;Trusted_Connection=True;";
