💰 Bütçe Yönetim ve Otomasyon Sistemi

Bu proje, KTÜ Bilgisayar Programcılığı eğitimim sürecimde geliştirdiğim; veritabanı mimarisi, backend güvenliği ve otomasyon temellerini içeren ileri seviye bir C# konsol uygulamasıdır.

Proje, standart veri işlemlerinin ötesine geçerek; kurumsal yazılım mimarilerinde kullanılan Stored Procedures (Saklı Yordamlar), Triggers (Tetikleyiciler) ve Nesne Yönelimli Programlama (OOP) yapıları üzerine inşa edilmiştir.

🚀 Proje Mimarisi ve Teknik Yetenekler

1. Katmanlı Mimari (OOP & Separation of Concerns)

Eski Yapı: Tüm kodların Program.cs içinde olduğu, yönetimi zor yapı.

Yeni Yapı: İş mantığı ve veritabanı erişim kodları HarcamaYonetimi sınıfına taşınarak soyutlanmıştır. Bu sayede kodun okunabilirliği ve güvenliği artırılmıştır.

2. Veritabanı Güvenliği (Stored Procedures)

SQL sorguları C# kodunun içine gömülmek yerine, SQL Server üzerinde Stored Procedure olarak saklanmıştır.

Avantaj: SQL Injection saldırılarına karşı tam koruma sağlar ve sunucu tarafında derlendiği için daha performanslı çalışır.

3. Otomatik Loglama Sistemi (SQL Triggers)

Uygulama üzerinden bir harcama silindiğinde, C# kodundan bağımsız çalışan bir Database Trigger devreye girer.

Silinen veri, otomatik olarak SILINENHARCAMALAR tablosuna yedeklenir. Bu sistem, veri kaybını önleyen bir "Veri Ajanı" görevi görür.

🛠️ Kurulum ve Veritabanı Scriptleri

Projeyi çalıştırmadan önce SQL Server Management Studio (SSMS) üzerinde aşağıdaki kodları çalıştırarak gerekli veritabanı altyapısını kurunuz.

NOT: Tablo ve kolon isimleri proje standartlarına uygun olarak BÜYÜK HARF ile tasarlanmıştır.

1. Veritabanı ve Tablolar

CREATE DATABASE ButceDB;
GO
USE ButceDB;

-- Ana Harcama Tablosu
CREATE TABLE HARCAMALAR (
    ID INT PRIMARY KEY IDENTITY(1,1),
    BASLIK NVARCHAR(50),
    TUTAR DECIMAL(18,2),
    TARIH DATETIME DEFAULT GETDATE(),
    KATEGORI NVARCHAR(30)
);

-- Silinen Verilerin Tutulduğu Yedek (Log) Tablosu
CREATE TABLE SILINENHARCAMALAR (
    LOGID INT PRIMARY KEY IDENTITY(1,1),
    ESKIID INT,
    BASLIK NVARCHAR(50),
    TUTAR DECIMAL(18,2),
    SILINMETARIHI DATETIME DEFAULT GETDATE()
);
GO


2. Saklı Yordamlar (Stored Procedures)

-- Ekleme İşlemi
CREATE PROCEDURE sp_HarcamaEkle
    @Baslik NVARCHAR(50),
    @Tutar DECIMAL(18,2),
    @Kategori NVARCHAR(30)
AS
BEGIN
    INSERT INTO HARCAMALAR (BASLIK, TUTAR, TARIH, KATEGORI)
    VALUES (@Baslik, @Tutar, GETDATE(), @Kategori)
END
GO

-- Silme İşlemi
CREATE PROCEDURE sp_HarcamaSil
    @Id INT
AS
BEGIN
    DELETE FROM HARCAMALAR WHERE ID = @Id
END
GO

-- Güncelleme İşlemi
CREATE PROCEDURE sp_HarcamaGuncelle
    @Id INT,
    @Baslik NVARCHAR(50),
    @Tutar DECIMAL(18,2),
    @Kategori NVARCHAR(30)
AS
BEGIN
    UPDATE HARCAMALAR 
    SET BASLIK = @Baslik, TUTAR = @Tutar, KATEGORI = @Kategori 
    WHERE ID = @Id
END
GO


3. Otomasyon Tetikleyicisi (Trigger)

CREATE TRIGGER TRG_HARCAMASILININCE
ON HARCAMALAR
AFTER DELETE
AS
BEGIN
    -- Silinen veriyi yakala ve yedek tablosuna aktar
    INSERT INTO SILINENHARCAMALAR (ESKIID, BASLIK, TUTAR)
    SELECT ID, BASLIK, TUTAR FROM DELETED
END
GO
