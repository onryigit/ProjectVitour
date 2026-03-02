# 🌍 Tour & Reservation Management Platform

ProjectVitour ASP.NET Core MVC (.NET 10) ve MongoDB tabanlı kapsamlı bir tur, destinasyon ve rezervasyon yönetim platformudur.

Bu proje sadece standart CRUD işlemlerini değil; gelişmiş raporlama, yapay zeka entegrasyonu, asenkron veritabanı yönetimi ve çoklu dil desteği gibi gerçek dünya senaryolarını içerir.

## 🚀 Proje Özellikleri

### 👥 Son Kullanıcı (UI) Deneyimi
* **Gelişmiş Çoklu Dil & Para Birimi Sistemi:** Cookie tabanlı altyapı ile TR/EN/DE dil seçenekleri. Sayfa içeriği ve tur fiyatlandırmaları (para birimleri) seçilen dile göre dinamik olarak anında değişir.
* **Dinamik Tur Listeleme & Paging:** MongoDB'den asenkron çekilen tur verileri, sayfa başına 6 öğe düşecek şekilde performanslı bir sayfalama (Paging) yapısıyla sunulur.
* **İnteraktif Tur Detayları:** Turlara ait *Information, Tour Planning, Reviews* ve *Shot Gallery* sekmeleri dinamik olarak yönetilir.
* **Gemini AI Harita & Bütçe Asistanı:** Klasik Google Haritalar yerine, Google Gemini API kullanılarak üretilmiş "Pixar animasyon" tarzı statik bölge görcelleri entegre edilmiştir. Ayrıca gizli maliyetleri hesaplayan bir Yapay Zeka Bütçe Asistanı bulunur.
* **Güvenli Rezervasyon Akışı:** Kullanıcı rezervasyon yaparken anlık kapasite (kontenjan) kontrolü yapılır. İşlem başarılı olduğunda **MailKit** aracılığıyla otomatik bilgilendirme e-postası gönderilir.

### ⚙️ Admin Panel & Operasyonel Yönetim
* **Modern & Modüler Yönetim:** Tailwind CSS ve yapay zeka desteğiyle sıfırdan tasarlanmış modern bir admin arayüzü. Sol menü ve tüm terimler tamamen Türkçeleştirilmiştir.
* **Kapsamlı Yönetim (CRUD):** Turlar, Kategoriler, Destinasyonlar, Yorumlar ve Rezervasyonlar için tam yetkili yönetim ekranları.
* **Gelişmiş Raporlama:** Rezervasyon listeleri tek tıkla **ClosedXML** ile Excel'e veya **iText7** ile PDF formatına dönüştürülerek dışa aktarılabilir.
* **Dashboard İstatistikleri:** Sistemdeki gelir, popüler turlar ve genel rezervasyon durumlarını gösteren özet ekran.

## 🛠️ Kullanılan Teknolojiler ve Mimari

Proje, tek katmanlı görünmesine rağmen kendi içinde `Services`, `Entities` ve `DTOs` klasör yapılarıyla **Interface Segregation** prensibine uygun, temiz ve sürdürülebilir bir mimariyle kurgulanmıştır.

* **Backend:** ASP.NET Core MVC (.NET 10), C#
* **Veritabanı:** MongoDB (Asenkron `async/await` mimarisi ile `MongoDB.Driver` kullanımı)
* **Yapay Zeka Entegrasyonu:** Google Gemini API
* **Araçlar & Kütüphaneler:**
  * AutoMapper (DTO dönüşümleri için)
  * ClosedXML (Excel export)
  * iText7 (PDF export)
  * MailKit (E-posta servisleri)
* **Frontend:** HTML5, Bootstrap, Tailwind CSS (Admin Panel)

## 📸 Ekran Görüntüleri
<img width="1906" height="951" alt="111" src="https://github.com/user-attachments/assets/3c5b8ba6-812e-4861-b8bb-543ff5d18fba" />
<img width="1912" height="948" alt="dashboard" src="https://github.com/user-attachments/assets/7e4029ef-3ae5-4921-85d1-178e1e378b6f" />
<img width="1903" height="948" alt="harita" src="https://github.com/user-attachments/assets/7f960d51-e910-4ad2-99d7-04bc4ca9f009" />
<img width="1905" height="944" alt="3" src="https://github.com/user-attachments/assets/4d06ef77-3dc2-42f8-b04a-23bf0911041f" />
<img width="1892" height="950" alt="4" src="https://github.com/user-attachments/assets/06ee87cd-3d4f-4122-bab8-5f5948cba930" />
<img width="1883" height="862" alt="5" src="https://github.com/user-attachments/assets/7a6ba143-36e0-4e1b-959a-0a2248e15da8" />
<img width="1901" height="948" alt="turdetay2" src="https://github.com/user-attachments/assets/82fc4f45-5dc5-4ae4-9924-a13cd9529080" />
<img width="1872" height="892" alt="mail" src="https://github.com/user-attachments/assets/8f62c940-edc5-4dbc-b134-3e1997b81a83" />
<img width="1898" height="943" alt="iletişim" src="https://github.com/user-attachments/assets/889ce780-9d74-456f-9bc4-e820b1e3ab33" />
<img width="1905" height="952" alt="rezervasyon" src="https://github.com/user-attachments/assets/09f31125-82a3-4049-b238-0e21a275fc61" />
<img width="1891" height="921" alt="rezervasyon2" src="https://github.com/user-attachments/assets/c931961d-b752-4152-bfac-6c200a28b184" />
<img width="1906" height="947" alt="turlar" src="https://github.com/user-attachments/assets/451e0e6f-0acf-46ca-959b-627378647b3a" />
<img width="1915" height="953" alt="yorumdetay" src="https://github.com/user-attachments/assets/aea20f2f-3c7c-4cb6-a608-f709e64fded6" />
<img width="1919" height="947" alt="rehberler" src="https://github.com/user-attachments/assets/900c9cb7-33d9-4f9f-bf63-f63d24eebdc5" />
<img width="1915" height="949" alt="destinasyonlar" src="https://github.com/user-attachments/assets/ec4840ac-ab6c-4569-b626-ca1b62ce3d06" />
<img width="1904" height="942" alt="tur ekle" src="https://github.com/user-attachments/assets/fcb8890d-b1a6-489b-94a1-9c1c9aca0219" />
<img width="1905" height="950" alt="yorumlar" src="https://github.com/user-attachments/assets/7cef69a9-6a19-4cc6-af3f-f987a1abdca7" />
<img width="1908" height="949" alt="tur güncelle" src="https://github.com/user-attachments/assets/af55598c-1ea2-438e-a6c4-d6b12bc8ad9c" />
<img width="1913" height="948" alt="rezervasyonlaradmin" src="https://github.com/user-attachments/assets/4da5acbc-4c11-4b21-9f90-3de07f20875f" />
<img width="801" height="954" alt="rezervasyonpdf" src="https://github.com/user-attachments/assets/34f74bdf-7308-4ced-9155-11c55f34e307" />














