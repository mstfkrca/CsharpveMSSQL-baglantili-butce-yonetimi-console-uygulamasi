using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ButceYonetımı
{
    internal class HarcamaYonetımı
    {
        static string connectionString = "Server=192.168.1.156,1433; Database=ButceDB; User Id=sa; Password=1234; TrustServerCertificate=True;";



        public void HarcamalariListele()
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

        public void HarcamaEkle()
        {
            Console.Write("Harcama başlığı: ");
            string baslik = Console.ReadLine();

            Console.Write("Tutar giriniz : ");
            decimal tutar = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Kategori giriniz : ");
            string kategori = Console.ReadLine();

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();

                    
                    string sorgu = "sp_HarcamaEkle";

                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        
                        komut.CommandType = System.Data.CommandType.StoredProcedure;

                        
                        komut.Parameters.AddWithValue("@Baslik", baslik);
                        komut.Parameters.AddWithValue("@Tutar", tutar);
                        komut.Parameters.AddWithValue("@Kategori", kategori);
                        

                        int etkilenenSatir = komut.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                            Console.WriteLine("Harcama başarıyla kaydedildi!");
                        else
                            Console.WriteLine("Bir sorun oluştu.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }

        public void HarcamaSil()
        {
            Console.Write("Silinecek harcamanın ID numarasını giriniz: ");
            
            int id = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();

                    // ARTIK PROSEDÜR KULLANIYORUZ
                    using (SqlCommand komut = new SqlCommand("sp_HarcamaSil", baglanti))
                    {
                        komut.CommandType = System.Data.CommandType.StoredProcedure;
                        komut.Parameters.AddWithValue("@Id", id);

                        int etkilenenSatir = komut.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                            Console.WriteLine("Harcama başarıyla silindi!");
                        else
                            Console.WriteLine("Belirtilen ID bulunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }

        public void HarcamaGuncelle()
        {
            Console.Write("Güncellenecek ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Yeni Başlık: ");
            string baslik = Console.ReadLine();

            Console.Write("Yeni Tutar: ");
            decimal tutar = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Yeni Kategori: ");
            string kategori = Console.ReadLine();

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();

                    
                    using (SqlCommand komut = new SqlCommand("sp_HarcamaGuncelle", baglanti))
                    {
                        komut.CommandType = System.Data.CommandType.StoredProcedure;

                        // Parametreleri prosedüre yolluyoruz
                        komut.Parameters.AddWithValue("@Id", id);
                        komut.Parameters.AddWithValue("@Baslik", baslik);
                        komut.Parameters.AddWithValue("@Tutar", tutar);
                        komut.Parameters.AddWithValue("@Kategori", kategori);

                        int etkilenenSatir = komut.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                            Console.WriteLine("Harcama başarıyla güncellendi!");
                        else
                            Console.WriteLine("Belirtilen ID bulunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }


        
        public void RaporlariGetir()
        {
            Console.WriteLine("\n--- GENEL DURUM RAPORU ---");

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();

                    // 1. Toplam Harcama Tutarı (SUM)
                    // ExecuteScalar: Tek bir değer (örneğin sadece toplam tutarı) döndüren sorgular için kullanılır.
                    SqlCommand komutToplam = new SqlCommand("SELECT SUM(TUTAR) FROM HARCAMALAR", baglanti);
                    // Eğer tablo boşsa NULL döner, hata vermemesi için kontrol ediyoruz:
                    object sonucToplam = komutToplam.ExecuteScalar();
                    decimal toplamTutar = (sonucToplam != DBNull.Value) ? Convert.ToDecimal(sonucToplam) : 0;

                    // 2. Toplam Kayıt Sayısı (COUNT)
                    SqlCommand komutAdet = new SqlCommand("SELECT COUNT(*) FROM HARCAMALAR", baglanti);
                    int kayitSayisi = Convert.ToInt32(komutAdet.ExecuteScalar());

                    // 3. En Yüksek Harcama (MAX)
                    SqlCommand komutMax = new SqlCommand("SELECT MAX(TUTAR) FROM HARCAMALAR", baglanti);
                    object sonucMax = komutMax.ExecuteScalar();
                    decimal enYuksek = (sonucMax != DBNull.Value) ? Convert.ToDecimal(sonucMax) : 0;

                    Console.WriteLine($"\n Toplam Harcanan Para: {toplamTutar} TL");
                    Console.WriteLine($" Toplam İşlem Adedi : {kayitSayisi} adet");
                    Console.WriteLine($" En Pahalı Harcama  : {enYuksek} TL");

                    // Ortalama (C# tarafında hesaplayalım)
                    if (kayitSayisi > 0)
                    {
                        Console.WriteLine($"  Ortalama Harcama   : {(toplamTutar / kayitSayisi).ToString("0.00")} TL");
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
