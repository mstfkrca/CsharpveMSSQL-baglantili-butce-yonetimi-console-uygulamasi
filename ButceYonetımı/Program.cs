using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ButceYonetımı
{
    internal class Program
    {


        

        static string connectionString = "Server=192.168.1*.***,14**; Database=ButceDB; User Id=******; Password=*******; TrustServerCertificate=True;";
        static void Main(string[] args)
        {
            bool devam = true;

            while (devam)
            {
                Console.Clear(); // Her işlemden sonra ekranı temizler
                Console.WriteLine("---  BÜTÇE YÖNETİM SİSTEMİ  ---");
                Console.WriteLine("1. Harcamaları Listele");
                Console.WriteLine("2. Yeni Harcama Ekle");
                Console.WriteLine("3. Harcama Sil");
                Console.WriteLine("4. Harcama Güncelle");
                Console.WriteLine("5. Çıkış");
                Console.Write("\nSeçiminiz (1-5): ");

                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        HarcamalariListele();
                        Bekle(); // Listeyi görebilmek için bekletme metodu
                        break;
                    case "2":
                        HarcamaEkle();
                        Bekle();
                        break;
                    case "3":
                        HarcamalariListele(); // Önce listeyi görsün ki ID seçebilsin
                        HarcamaSil();
                        Bekle();
                        break;
                    case "4":
                        HarcamalariListele(); // Önce listeyi görsün
                        HarcamaGuncelle();
                        Bekle();
                        break;
                    case "5":
                        devam = false;
                        Console.WriteLine("Çıkış yapılıyor...");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim! Tekrar deneyin.");
                        Bekle();
                        break;
                }
            }
        }

        // Kullanıcı bir tuşa basana kadar ekranın kapanmamasını sağlayan yardımcı metot
        static void Bekle()
        {
            Console.WriteLine("\nAna menüye dönmek için bir tuşa basın...");
            Console.ReadKey();
        }

        static void HarcamalariListele()
        {
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    string sorgu = "SELECT * FROM HARCAMALAR";
                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        using (SqlDataReader okuyucu = komut.ExecuteReader())
                        {
                            Console.WriteLine("\nKayıtlı Harcamalar:\n");
                            while (okuyucu.Read())
                            {
                                Console.WriteLine("ID: {0}, Başlık: {1}, Tutar: {2}, Kategori: {3}",
                                    okuyucu["ID"], okuyucu["BASLIK"], okuyucu["TUTAR"], okuyucu["KATEGORI"]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }

        static void HarcamaEkle()
        {
            Console.Write("Harcama başlığı: ");
            string baslik = Console.ReadLine();

            //tutar decimal

            Console.Write("Tutar giriniz : ");
            decimal tutar = Convert.ToDecimal(Console.ReadLine());




            //kategori string

            Console.Write("Kategori giriniz : ");
            string kategori = Console.ReadLine();



            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open(); // Kapıyı çal ve içeri gir

                    //sql sorgusu
                    string sorgu = "INSERT INTO HARCAMALAR (BASLIK, TUTAR, KATEGORI) VALUES (@baslik, @tutar, @kategori)";

                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        //parametreleri ekleme
                        komut.Parameters.AddWithValue("@baslik", baslik);
                        komut.Parameters.AddWithValue("@tutar", tutar);
                        komut.Parameters.AddWithValue("@kategori", kategori);

                        int etkilenenSatir = komut.ExecuteNonQuery(); //sorguyu çalıştır ve etkilenen satır sayısını al

                        if (etkilenenSatir > 0)
                            Console.WriteLine("Harcama başarıyla veritabanına kaydedildi!");
                        else
                            Console.WriteLine("Bir sorun oluştu, kaydedilemedi.");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }

        static void HarcamaSil()
        {
            Console.Write("Silinecek harcamanın ID numarasını giriniz: ");

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    int id = Convert.ToInt32(Console.ReadLine());
                    string sorgu = "DELETE FROM HARCAMALAR WHERE ID = @id";
                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@id", id);
                        int etkilenenSatir = komut.ExecuteNonQuery();
                        if (etkilenenSatir > 0)
                            Console.WriteLine("Harcama başarıyla silindi!");
                        else
                            Console.WriteLine("Belirtilen ID'ye sahip harcama bulunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }

        static void HarcamaGuncelle()
        {
            Console.Write("Güncellenecek harcamanın ID numarasını giriniz: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Yeni harcama başlığı: ");
            string baslik = Console.ReadLine();
            Console.Write("Yeni tutar: ");
            decimal tutar = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Yeni kategori: ");
            string kategori = Console.ReadLine();
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    string sorgu = "UPDATE HARCAMALAR SET BASLIK = @baslik, TUTAR = @tutar, KATEGORI = @kategori WHERE ID = @id";
                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@baslik", baslik);
                        komut.Parameters.AddWithValue("@tutar", tutar);
                        komut.Parameters.AddWithValue("@kategori", kategori);
                        komut.Parameters.AddWithValue("@id", id);
                        int etkilenenSatir = komut.ExecuteNonQuery();
                        if (etkilenenSatir > 0)
                            Console.WriteLine("Harcama başarıyla güncellendi!");
                        else
                            Console.WriteLine("Belirtilen ID'ye sahip harcama bulunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }
    }
}
