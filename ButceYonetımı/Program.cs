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





        static void Main(string[] args)
        {




            HarcamaYonetımı yonetici = new HarcamaYonetımı();

            bool devam = true;

            while (devam)
            {
                Console.Clear();
                Console.WriteLine("---  BÜTÇE YÖNETİM SİSTEMİ  ---");
                Console.WriteLine("1. Harcamaları Listele");
                Console.WriteLine("2. Yeni Harcama Ekle");
                Console.WriteLine("3. Harcama Sil");
                Console.WriteLine("4. Harcama Güncelle");
                Console.WriteLine("5. Raporları Göster");
                Console.WriteLine("6. Çıkış");
                Console.Write("\nSeçiminiz (1-6): ");

                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        yonetici.HarcamalariListele();
                        Bekle(); // Listeyi görebilmek için bekletme metodu
                        break;
                    case "2":
                        yonetici.HarcamaEkle();
                        Bekle();
                        break;
                    case "3":
                        yonetici.HarcamalariListele();
                        yonetici.HarcamaSil();
                        Bekle();
                        break;
                    case "4":
                        yonetici.HarcamalariListele();
                        yonetici.HarcamaGuncelle();
                        Bekle();
                        break;
                    case "5":
                        yonetici.RaporlariGetir();
                        Bekle();
                        break;
                    case "6":
                        devam = false;
                        Console.WriteLine("Çıkış yapılıyor...");
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


    }
}
