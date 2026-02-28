using MongoDB.Bson;
using MongoDB.Driver;
using ProjectVitour.Entities;
using ProjectVitour.Settings;
using System;
using System.Collections.Generic;

namespace ProjectVitour
{
    public static class DataSeeder
    {
        public static void ClearAndSeed(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);

            // Koleksiyonları al
            var destCol = db.GetCollection<Destination>(settings.DestinationCollectionName);
            var tourCol = db.GetCollection<Tour>(settings.TourCollectionName);
            var categoryCol = db.GetCollection<Category>(settings.CategoryCollectionName);
            var guideCol = db.GetCollection<Guide>(settings.GuideCollectionName);
            var reviewCol = db.GetCollection<Review>(settings.ReviewCollectionName);
            var planCol = db.GetCollection<TourPlan>(settings.TourPlanCollectionName);
            var imgCol = db.GetCollection<TourImage>(settings.TourImageCollectionName);

            // 1. Önceki tüm verileri (hatalı görseller vb.) temizliyoruz.
            db.DropCollection(settings.DestinationCollectionName);
            db.DropCollection(settings.TourCollectionName);
            db.DropCollection(settings.CategoryCollectionName);
            db.DropCollection(settings.GuideCollectionName);
            db.DropCollection(settings.ReviewCollectionName);
            db.DropCollection(settings.TourPlanCollectionName);
            db.DropCollection(settings.TourImageCollectionName);

            // Koleksiyonları tekrar bağlıyoruz
            destCol = db.GetCollection<Destination>(settings.DestinationCollectionName);
            tourCol = db.GetCollection<Tour>(settings.TourCollectionName);
            categoryCol = db.GetCollection<Category>(settings.CategoryCollectionName);
            guideCol = db.GetCollection<Guide>(settings.GuideCollectionName);
            reviewCol = db.GetCollection<Review>(settings.ReviewCollectionName);
            planCol = db.GetCollection<TourPlan>(settings.TourPlanCollectionName);
            imgCol = db.GetCollection<TourImage>(settings.TourImageCollectionName);

            // 2. Kategoriler (Categories)
            var cat1 = new Category { CategoryId = ObjectId.GenerateNewId().ToString(), CategoryName = "Kültür Turları", CategoryStatus = true };
            var cat2 = new Category { CategoryId = ObjectId.GenerateNewId().ToString(), CategoryName = "Doğa Turları", CategoryStatus = true };
            var cat3 = new Category { CategoryId = ObjectId.GenerateNewId().ToString(), CategoryName = "Yurtdışı Turları", CategoryStatus = true };
            categoryCol.InsertMany(new[] { cat1, cat2, cat3 });

            // 3. Rehberler (Guides)
            var guide1 = new Guide { GuideID = ObjectId.GenerateNewId().ToString(), FullName = "Bora Yılmaz", Title = "Tarih ve Kültür Uzmanı", ImageUrl = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=400&q=80", InstagramUrl = "https://instagram.com", TwitterUrl = "https://twitter.com", Status = true };
            var guide2 = new Guide { GuideID = ObjectId.GenerateNewId().ToString(), FullName = "Selin Demir", Title = "Doğa ve Doğa Sporları Rehberi", ImageUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400&q=80", InstagramUrl = "https://instagram.com", TwitterUrl = "https://twitter.com", Status = true };
            guideCol.InsertMany(new[] { guide1, guide2 });

            // 4. Destinasyonlar (Destinations)
            var destCap = new Destination { DestinationID = ObjectId.GenerateNewId().ToString(), CityName = "Nevşehir", CountryName = "Türkiye", ImageUrl = "https://images.unsplash.com/photo-1604084797086-63d1cc96c429?w=800&q=80", Description = "Peri bacaları ve sıcak hava balonları ile büyüleyici bir deneyim.", Status = true };
            var destKar = new Destination { DestinationID = ObjectId.GenerateNewId().ToString(), CityName = "Rize", CountryName = "Türkiye", ImageUrl = "https://images.unsplash.com/photo-1542281286-9e0a16bb7366?w=800&q=80", Description = "Yemyeşil yaylaları ve hırçın dereleriyle doğanın kalbi.", Status = true };
            var destRome = new Destination { DestinationID = ObjectId.GenerateNewId().ToString(), CityName = "Roma", CountryName = "İtalya", ImageUrl = "https://images.unsplash.com/photo-1552832230-c0197dd311b5?w=800&q=80", Description = "Tarihin ve sanatın başkenti, antik kalıntılarla dolu eşsiz bir şehir.", Status = true };
            destCol.InsertMany(new[] { destCap, destKar, destRome });

            // 5. Turlar (Tours)
            var tourCap = new Tour
            {
                TourID = ObjectId.GenerateNewId().ToString(),
                Title = "Büyüleyici Kapadokya Turu",
                Description = "3 gün 2 gece sürecek bu eşsiz turda peri bacalarının efsanevi oluşumlarına şahit olacak, sabahın ilk ışıklarıyla sıcak hava balonlarının gökyüzünde oluşturduğu görsel şöleni izleyeceksiniz. Butik kaya otellerde konaklama ve yöresel şarap tadımları fiyata dâhildir.",
                CoverImageUrl = "https://images.unsplash.com/photo-1604084797086-63d1cc96c429?w=800&q=80",
                Badge = "Popular",
                DayCount = 3,
                Capacity = 20,
                Price = 180,
                IsStatus = true,
                Location = "Nevşehir, TR",
                DestinationID = destCap.DestinationID,
                MapLocationImageUrl = "https://images.unsplash.com/photo-1524661135-423995f22d0b?w=800&q=80"
            };

            var tourKar = new Tour
            {
                TourID = ObjectId.GenerateNewId().ToString(),
                Title = "Karadeniz Yaylaları Keşfi",
                Description = "Ayder ve Uzungöl başta olmak üzere Karadeniz'in en güzel yaylalarını keşfedeceğimiz bu doğa turunda, yöresel lezzetlerin tadına bakacak ve oksijene doyacaksınız. Zilkale ve Palovit Şelalesi gezileri programımıza dâhildir.",
                CoverImageUrl = "https://images.unsplash.com/photo-1542281286-9e0a16bb7366?w=800&q=80",
                Badge = "New",
                DayCount = 4,
                Capacity = 15,
                Price = 220,
                IsStatus = true,
                Location = "Rize, TR",
                DestinationID = destKar.DestinationID,
                MapLocationImageUrl = "https://images.unsplash.com/photo-1524661135-423995f22d0b?w=800&q=80"
            };

            var tourRome = new Tour
            {
                TourID = ObjectId.GenerateNewId().ToString(),
                Title = "Antik Roma ve Vatikan",
                Description = "Kolezyum'dan Trevi Çeşmesi'ne, Roma'nın tüm tarihi dokusunu profesyonel rehberlerimiz eşliğinde adım adım gezeceksiniz. Gerçek bir Avrupa kültürü ve Rönesans sanatı yolculuğu!",
                CoverImageUrl = "https://images.unsplash.com/photo-1552832230-c0197dd311b5?w=800&q=80",
                Badge = "Limited",
                DayCount = 5,
                Capacity = 30,
                Price = 650,
                IsStatus = true,
                Location = "Roma, IT",
                DestinationID = destRome.DestinationID,
                MapLocationImageUrl = "https://images.unsplash.com/photo-1524661135-423995f22d0b?w=800&q=80"
            };

            tourCol.InsertMany(new[] { tourCap, tourKar, tourRome });

            // 6. Tur Programları (Tour Plans)
            var plans = new List<TourPlan>
            {
                new TourPlan { TourPlanID = ObjectId.GenerateNewId().ToString(), TourID = tourCap.TourID, DayNumber = 1, Title = "Bölgeye Varış ve Otele Yerleşme", Description = "Kayseri/Nevşehir havalimanında karşılama, Göreme'de butik kaya otele yerleşme ve gün batımında Kızılçukur Vadisi yürüyüşü." },
                new TourPlan { TourPlanID = ObjectId.GenerateNewId().ToString(), TourID = tourCap.TourID, DayNumber = 2, Title = "Balon Turu ve Ihlara Vadisi", Description = "Sabah 05:00'te sıcak hava balonu uçuşu. Kahvaltı sonrası Ihlara Vadisi'ne hareket, dere kenarında yürüyüş ve Melendiz çayı kenarında öğle yemeği." },
                new TourPlan { TourPlanID = ObjectId.GenerateNewId().ToString(), TourID = tourCap.TourID, DayNumber = 3, Title = "Yeraltı Şehri ve Dönüş", Description = "Derinkuyu Yeraltı Şehri gezisi, Avanos'ta çömlek atölyesi ziyareti ve havalimanına transfer." },

                new TourPlan { TourPlanID = ObjectId.GenerateNewId().ToString(), TourID = tourRome.TourID, DayNumber = 1, Title = "Roma'ya Merhaba", Description = "Roma havalimanında karşılama, otele transfer. Akşamüstü İspanyol Merdivenleri ve Navona Meydanı turu." },
                new TourPlan { TourPlanID = ObjectId.GenerateNewId().ToString(), TourID = tourRome.TourID, DayNumber = 2, Title = "Kolezyum ve Antik Roma", Description = "Kolezyum iç gezisi, Roma Forumu ve Palatino Tepesi'nin rehber eşliğinde keşfedilmesi." }
            };
            planCol.InsertMany(plans);

            // 7. Tur Görselleri (Shot Gallery)
            var images = new List<TourImage>
            {
                new TourImage { TourImageID = ObjectId.GenerateNewId().ToString(), TourID = tourCap.TourID, ImageUrl = "https://images.unsplash.com/photo-1594957597534-118f6c58908f?w=800&q=80" },
                new TourImage { TourImageID = ObjectId.GenerateNewId().ToString(), TourID = tourCap.TourID, ImageUrl = "https://images.unsplash.com/photo-1527631746610-bca00a040d60?w=800&q=80" },
                new TourImage { TourImageID = ObjectId.GenerateNewId().ToString(), TourID = tourCap.TourID, ImageUrl = "https://images.unsplash.com/photo-1604084797086-63d1cc96c429?w=800&q=80" },

                new TourImage { TourImageID = ObjectId.GenerateNewId().ToString(), TourID = tourRome.TourID, ImageUrl = "https://images.unsplash.com/photo-1552832230-c0197dd311b5?w=800&q=80" },
                new TourImage { TourImageID = ObjectId.GenerateNewId().ToString(), TourID = tourRome.TourID, ImageUrl = "https://images.unsplash.com/photo-1531572753322-ad0110ce36f1?w=800&q=80" }
            };
            imgCol.InsertMany(images);

            // 8. Müşteri Yorumları (Reviews)
            var reviews = new List<Review>
            {
                new Review { ReviewId = ObjectId.GenerateNewId().ToString(), TourId = tourCap.TourID, NameSurname = "Ahmet Yılmaz", Detail = "Kapadokya turu tek kelimeyle kusursuzdu! Rehberimiz Bora Bey'in bilgi birikimi ve otelin kalitesi bizi çok mutlu etti. Balon turunu kesinlikle öneririm.", GuideRating = 5, AccommodationRating = 5, TransportRating = 4, ComfortRating = 5, ReviewDate = DateTime.Now.AddDays(-15), Status = true },
                new Review { ReviewId = ObjectId.GenerateNewId().ToString(), TourId = tourCap.TourID, NameSurname = "Cansu Aydın", Detail = "Program çok güzeldi ama otobüs yolculuğu biraz yorucuydu. Yine de Ihlara Vadisi yürüyüşüne bayıldım.", GuideRating = 5, AccommodationRating = 4, TransportRating = 3, ComfortRating = 4, ReviewDate = DateTime.Now.AddDays(-5), Status = true },
                
                new Review { ReviewId = ObjectId.GenerateNewId().ToString(), TourId = tourRome.TourID, NameSurname = "Kemal Tekin", Detail = "Roma gezimiz bir rüya gibiydi. Kolezyum'da sıra beklemeden içeri girmemiz büyük bir ayrıcalıktı. Yemekler de harikaydı.", GuideRating = 5, AccommodationRating = 4, TransportRating = 5, ComfortRating = 5, ReviewDate = DateTime.Now.AddDays(-2), Status = true }
            };
            reviewCol.InsertMany(reviews);
        }
    }
}