using System;
using System.IO;

namespace EvClass
{
    public abstract class Ev
    {
        /*
         *  @"Belgeler/Emlak" -> Çalýþmayabilir, Belgelere veri kaydetme amaçlanmýþtýr.
         */

        private int odaSayisi, katNumarasi, evAlani, emlakNo;
        private Boolean evAktifDurum = false;
        private string semt;
        DateTime yapimTarihi;

        public int OdaSayisi
        {
            get
            {
                return odaSayisi;
            }

            set
            {
                if (odaSayisi < 0)
                {
                    OdaSayisiLog();
                    odaSayisi = 0;
                }
                else
                {
                    OdaSayisiLog();
                    odaSayisi = value;
                }
            }
        }

        //Oda sayisi log tutma iþlemleri
        private void OdaSayisiLog()
        {
            if (!Directory.Exists(@"Belgeler/Emlak"))
            {
                Directory.CreateDirectory(@"Belgeler/Emlak");
            }
            FileStream fs = new FileStream(@"Belgeler/Emlak/EvKayitLOG.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("Girilmek Ýstenen Deger|" + odaSayisi + "|Tarih|" + DateTime.Now);
            fs.Close(); sw.Close();
        }

        public int KatNumarasi
        {
            get
            {
                return katNumarasi;
            }

            set
            {
                katNumarasi = value;
            }
        }

        public int EvAlani
        {
            get
            {
                return evAlani;
            }

            set
            {
                evAlani = value;
            }
        }

        public int EmlakNo
        {
            get
            {
                return emlakNo;
            }

            set
            {
                emlakNo = value;
            }
        }

        public bool EvAktifDurum
        {
            get
            {
                return evAktifDurum;
            }

            set
            {
                evAktifDurum = value;
            }
        }

        public string Semt
        {
            get
            {
                return semt;
            }

            set
            {
                semt = value;
            }
        }

        //Yapým tarihi girildiðinde get blogu ev yaþýný döndürür.
        public DateTime YapimTarihi
        {
            get
            {
                return yapimTarihi;
            }

            set
            {
                yapimTarihi = value;
                TimeSpan ts;
                ts = DateTime.Now - YapimTarihi;
                yapimTarihi = Convert.ToDateTime(ts);
            }
        }

        public enum EvTur
        {
            Bilinmiyor,
            Daire,
            Bahçeli,
            Dubleks,
            Müstakil,
            Havuzlu,
            Apart
        }

        public abstract string EvBilgileri();
        public abstract string EvTurBilgisiGoruntule();
        public abstract int FiyatHesapla();
    }

    public class KiralikEv : Ev
    {
        public int depozito = 1000, kira = 1000;
        public EvTur evtur;

        public KiralikEv()
        {
            evtur = EvTur.Bilinmiyor;
        }

        public override string EvBilgileri()
        {
            return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", OdaSayisi, KatNumarasi, EvAlani, EmlakNo, EvAktifDurum, Semt, YapimTarihi, EvTurBilgisiGoruntule(), depozito, kira);
        }

        public override string EvTurBilgisiGoruntule()
        {
            return Convert.ToString(evtur);
        }

        // Fiyat hesaplama detaylarý sitedeki dökümanda yer almaktadýr.
        public override int FiyatHesapla()
        {
            try
            {
                FileStream fs = new FileStream(@"Belgeler/Emlak/room_cost.txt", FileMode.Append);
                StreamReader sr = new StreamReader(fs);

                if (sr.ReadLine() == "")
                    return kira = 200 * OdaSayisi;

                return kira = Convert.ToInt32(sr.ReadLine()) * OdaSayisi;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
    }

    public class SatilikEv : Ev
    {
        public int fiyat, evFiyatKatsayi = 1000;
        public EvTur evtur;

        public SatilikEv()
        {
            evtur = EvTur.Bilinmiyor;
        }

        public override string EvBilgileri()
        {
            return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", OdaSayisi, KatNumarasi, EvAlani, EmlakNo, EvAktifDurum, Semt, YapimTarihi, EvTurBilgisiGoruntule(), fiyat);
        }

        public override string EvTurBilgisiGoruntule()
        {
            return Convert.ToString(evtur);
        }

        //Satilik ev için deger *1000 baz alýndý
        public override int FiyatHesapla()
        {
            FileStream fs = new FileStream(@"Belgeler/Emlak/room_cost.txt", FileMode.Append);
            StreamReader sr = new StreamReader(fs);

            return fiyat = Convert.ToInt32(sr.ReadLine()) * evFiyatKatsayi;
        }
    }
}
