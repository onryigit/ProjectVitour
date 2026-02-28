using MongoDB.Bson;
using MongoDB.Driver;
using ProjectVitour.Entities;
using ProjectVitour.Settings;
using System;
using System.Collections.Generic;

namespace ProjectVitour.Data
{
    public static class SeedData
    {
        public static void Initialize(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);

            // Eski verileri tamamen uçuruyoruz
            db.DropCollection(settings.DestinationCollectionName);
            db.DropCollection(settings.TourCollectionName);
            db.DropCollection(settings.CategoryCollectionName);
            db.DropCollection(settings.GuideCollectionName);
            db.DropCollection(settings.ReviewCollectionName);
            db.DropCollection(settings.TourPlanCollectionName);
            db.DropCollection(settings.TourImageCollectionName);

            var destCol = db.GetCollection<Destination>(settings.DestinationCollectionName);
            var tourCol = db.GetCollection<Tour>(settings.TourCollectionName);
            var catCol = db.GetCollection<Category>(settings.CategoryCollectionName);
            var guideCol = db.GetCollection<Guide>(settings.GuideCollectionName);
            var reviewCol = db.GetCollection<Review>(settings.ReviewCollectionName);
            var planCol = db.GetCollection<TourPlan>(settings.TourPlanCollectionName);
            var imgCol = db.GetCollection<TourImage>(settings.TourImageCollectionName);

            Random rnd = new Random();

            // 1. Kategoriler
            var categories = new List<Category>
            {
                new Category { CategoryId = ObjectId.GenerateNewId().ToString(), CategoryName = "Kültür ve Tarih", CategoryStatus = true },
                new Category { CategoryId = ObjectId.GenerateNewId().ToString(), CategoryName = "Egzotik Plajlar", CategoryStatus = true },
                new Category { CategoryId = ObjectId.GenerateNewId().ToString(), CategoryName = "Doğa Maceraları", CategoryStatus = true },
                new Category { CategoryId = ObjectId.GenerateNewId().ToString(), CategoryName = "Lüks Balayı", CategoryStatus = true }
            };
            catCol.InsertMany(categories);

            // 2. Rehberler
            var guides = new List<Guide>
            {
                new Guide { GuideID = ObjectId.GenerateNewId().ToString(), FullName = "Prof. Dr. İlber Yılmaz", Title = "Arkeoloji ve Tarih Uzmanı", ImageUrl = "https://images.unsplash.com/photo-1560250097-0b93528c311a?w=400&q=80", InstagramUrl = "https://instagram.com", TwitterUrl = "https://twitter.com", Status = true },
                new Guide { GuideID = ObjectId.GenerateNewId().ToString(), FullName = "Selin Demirkaynak", Title = "Egzotik Rotalar Danışmanı", ImageUrl = "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=400&q=80", InstagramUrl = "https://instagram.com", TwitterUrl = "https://twitter.com", Status = true },
                new Guide { GuideID = ObjectId.GenerateNewId().ToString(), FullName = "Ateş Can", Title = "Doğa Sporları ve Tırmanış Rehberi", ImageUrl = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=400&q=80", InstagramUrl = "https://instagram.com", TwitterUrl = "https://twitter.com", Status = true }
            };
            guideCol.InsertMany(guides);

            // 3. Destinasyonlar
            var d1 = new Destination { DestinationID = ObjectId.GenerateNewId().ToString(), CityName = "Paris", CountryName = "Fransa", ImageUrl = "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?w=800&q=80", Description = "Aşk ve Sanatın Başkenti", Status = true };
            var d2 = new Destination { DestinationID = ObjectId.GenerateNewId().ToString(), CityName = "Roma", CountryName = "İtalya", ImageUrl = "https://images.unsplash.com/photo-1552832230-c0197dd311b5?w=800&q=80", Description = "Tarihin Kalbi", Status = true };
            var d3 = new Destination { DestinationID = ObjectId.GenerateNewId().ToString(), CityName = "Bali", CountryName = "Endonezya", ImageUrl = "https://images.unsplash.com/photo-1537996194471-e657df975ab4?w=800&q=80", Description = "Tropikal Cennet", Status = true };
            var d4 = new Destination { DestinationID = ObjectId.GenerateNewId().ToString(), CityName = "Kapadokya", CountryName = "Türkiye", ImageUrl = "https://images.unsplash.com/photo-1604084797086-63d1cc96c429?w=800&q=80", Description = "Masalsı Peribacaları", Status = true };
            var d5 = new Destination { DestinationID = ObjectId.GenerateNewId().ToString(), CityName = "Kyoto", CountryName = "Japonya", ImageUrl = "https://images.unsplash.com/photo-1493976040374-85c8e12f0c0e?w=800&q=80", Description = "Geleneksel Japon Ruhu", Status = true };
            var d6 = new Destination { DestinationID = ObjectId.GenerateNewId().ToString(), CityName = "Maldivler", CountryName = "Maldivler", ImageUrl = "https://images.unsplash.com/photo-1514282401089-ce3a571e2372?w=800&q=80", Description = "Turkuaz Sular", Status = true };
            
            destCol.InsertMany(new[] { d1, d2, d3, d4, d5, d6 });

            // 4. Elle Hazırlanmış 30 Farklı Tur (Eşsiz içerik ve garantili görseller)
            var tourDefinitions = new[]
            {
                // PARIS
                new { Title = "Paris: Romantizmin Başkentinde Sanat Turu", Desc = "Louvre müzesinden Şanzelize'ye, Paris'in sanatsal dokusunu keşfedeceğiniz eşsiz bir 4 günlük kaçamak.", Cover = "1499856871958-5b9627545d1a", Badge = "En Çok Satan", Dest = d1, Day = 4, Price = 1200 },
                new { Title = "Gizli Paris: Yerellerin Gözünden Fransa", Desc = "Montmartre'nin arka sokakları, butik kafeler ve Seine nehri kıyısında edebi bir yolculuğa çıkın.", Cover = "1511739001486-6bfe10ce785f", Badge = "Özel Konsept", Dest = d1, Day = 5, Price = 1450 },
                new { Title = "Paris Gurme Gezisi ve Şarap Tadımı", Desc = "Fransız mutfağının inceliklerini Michelin yıldızlı şeflerin atölyelerinde öğreneceğiniz lezzet dolu bir program.", Cover = "1509302846320-8128ba470771", Badge = "Yeni", Dest = d1, Day = 3, Price = 950 },
                new { Title = "Işıklar Şehri: Paris Gece Turu", Desc = "Sadece akşamları gerçekleşen bu özel turda, Eyfel'in ışıltısı eşliğinde Seine nehrinde akşam yemeği sizi bekliyor.", Cover = "1431274172761-fca41d930114", Badge = "Popüler", Dest = d1, Day = 2, Price = 800 },
                new { Title = "Paris Müzeleri: Rönesans'a Yolculuk", Desc = "Orsay ve Louvre müzelerine VIP bilet ile giriş imkanı sunan, tamamen sanat tarihine adanmış bir hafta sonu.", Cover = "1543305113-82b47ea48dc2", Badge = "Kültür Turu", Dest = d1, Day = 3, Price = 1100 },

                // ROMA
                new { Title = "Antik Roma ve Gladyatörlerin İzi", Desc = "Kolezyum'un yeraltı tünellerinden başlayarak Antik Roma Forumu'nda tarihin tozlu sayfalarını aralayacaksınız.", Cover = "1531572753322-ad0110ce36f1", Badge = "Çok Tercih Edilen", Dest = d2, Day = 4, Price = 1050 },
                new { Title = "Vatikan Sırları ve Sistina Şapeli", Desc = "Dünyanın en küçük ülkesi Vatikan'da Michelangelo'nun paha biçilemez eserlerini rehberimiz eşliğinde okuyun.", Cover = "1515542622106-78b28af7815b", Badge = "Sınırlı Kontenjan", Dest = d2, Day = 3, Price = 980 },
                new { Title = "İtalyan Lezzetleri: Roma Sokak Yemekleri", Desc = "Trastevere mahallesinde odun ateşinde pizza ve geleneksel gelato tadımlarıyla dolu bir gastronomi serüveni.", Cover = "1584967918940-05047734ea02", Badge = "Gurme", Dest = d2, Day = 3, Price = 850 },
                new { Title = "Aşıklar Çeşmesi ve Roma Geceleri", Desc = "Trevi Çeşmesi'nde dilek tutup, İspanyol Merdivenleri'nde sokak müzisyenlerini dinleyeceğiniz romantik rota.", Cover = "1563821035987-9bb32d84db8b", Badge = "Popüler", Dest = d2, Day = 4, Price = 1150 },
                new { Title = "Roma ve Floransa Hızlı Tren Turu", Desc = "İtalya'nın iki kalbini birleştiren bu turda, hızlı trenle hem Roma'nın antik dokusunu hem Floransa'nın sanatını yaşayın.", Cover = "1604580864964-0462f5d5b1a8", Badge = "Premium", Dest = d2, Day = 6, Price = 1800 },

                // BALI
                new { Title = "Bali Egzotik Balayı Paketi", Desc = "Özel havuzlu villalarda konaklama, deniz kenarında mum ışığında yemek ve geleneksel masajlarla dolu kusursuz balayı.", Cover = "1518548419970-58e3b4079ab2", Badge = "Balayı Özel", Dest = d3, Day = 7, Price = 2400 },
                new { Title = "Tapınaklar ve Kutsal Maymun Ormanı", Desc = "Bali'nin mistik inançlarını keşfedip, antik taş tapınaklarda yerel seremonilere şahitlik edeceğiniz derin bir yolculuk.", Cover = "1555400038-63f5ba517a47", Badge = "Popüler", Dest = d3, Day = 5, Price = 1600 },
                new { Title = "Bali Sörf ve Macera Kampı", Desc = "Kuta plajında profesyonel sörf eğitimleri ve orman derinliklerindeki gizli şelalelere ATV turları.", Cover = "1570222094114-d054a817e56b", Badge = "Macera", Dest = d3, Day = 6, Price = 1750 },
                new { Title = "Ubud Pirinç Terasları Yürüyüşü", Desc = "Yeşilin her tonunu görebileceğiniz ünlü pirinç teraslarında doğa yürüyüşü ve ünlü Bali salıncağı deneyimi.", Cover = "1537953773345-d172ccf13cf1", Badge = "Doğa", Dest = d3, Day = 4, Price = 1200 },
                new { Title = "Nusa Penida Adaları Tekne Turu", Desc = "Manta vatozlarıyla şnorkel yapabileceğiniz, kristal berraklığındaki koylarda geçecek inanılmaz bir deniz turu.", Cover = "1512552288040-3601ceb1202d", Badge = "Yeni", Dest = d3, Day = 3, Price = 950 },

                // KAPADOKYA
                new { Title = "Kapadokya: Gökyüzünde Balon Şöleni", Desc = "Sabahın ilk ışıklarıyla havalanan yüzlerce balonun arasında yerinizi alın. Peribacalarının üzerinden süzülmenin büyüsü.", Cover = "1641128324972-af3212f0f6bd", Badge = "Fırsat Ürünü", Dest = d4, Day = 3, Price = 450 },
                new { Title = "Yeraltı Şehirleri ve Tarihin İzleri", Desc = "Derinkuyu'nun derinliklerine inip binlerce yıl öncesinin yaşam alanlarını inceleyeceğiniz arkeolojik keşif turu.", Cover = "1527631746610-bca00a040d60", Badge = "Kültür Turu", Dest = d4, Day = 2, Price = 300 },
                new { Title = "Kapadokya Mağara Otel Deneyimi", Desc = "Tamamen kayalara oyulmuş, modern lüksle birleşen otantik mağara otellerde eşsiz bir konaklama ayrıcalığı.", Cover = "1594957597534-118f6c58908f", Badge = "Premium", Dest = d4, Day = 4, Price = 850 },
                new { Title = "Kızılvadi Atlı Safari Turu", Desc = "Gün batımında volkanik kayaların kızıla boyandığı anları at sırtında, profesyoneller eşliğinde yaşayın.", Cover = "1627894483216-2138fb692e32", Badge = "Macera", Dest = d4, Day = 2, Price = 350 },
                new { Title = "Avanos Çömlek Atölyesi ve Şarap", Desc = "Kızılırmak kenarında kendi çömleğinizi yaparken, bölgenin meşhur ev yapımı şaraplarının tadını çıkarın.", Cover = "1570125909232-eb263c188f7e", Badge = "Gurme", Dest = d4, Day = 3, Price = 500 },

                // KYOTO
                new { Title = "Kyoto: Sakura Zamanı Japonya", Desc = "Sokakları pembeye boyayan kiraz çiçekleri altında, tapınak bahçelerinde huzur dolu bir Japonya baharı.", Cover = "1545569341-9eb8b30979d9", Badge = "Popüler", Dest = d5, Day = 6, Price = 2200 },
                new { Title = "Samuray ve Geyşa Kültür Gezisi", Desc = "Gion bölgesinde geleneksel çay seremonilerine katılıp, Japon tarihinin en gizemli kültürlerini tanıyacaksınız.", Cover = "1528360983277-13d401cdc186", Badge = "Özel Konsept", Dest = d5, Day = 5, Price = 1900 },
                new { Title = "Arashiyama Bambu Ormanı Sessizliği", Desc = "Gökyüzüne uzanan devasa bambuların rüzgardaki sesini dinleyerek yapacağınız içsel bir arınma yürüyüşü.", Cover = "1578469645742-46cae010e5d4", Badge = "Doğa", Dest = d5, Day = 4, Price = 1600 },
                new { Title = "Altın Köşk (Kinkaku-ji) Gezisi", Desc = "Suya yansıyan muazzam mimarisiyle Altın Köşk ve Zen bahçelerinde gerçekleşen meditatif bir Japonya deneyimi.", Cover = "1624253321171-1be53e12f5f4", Badge = "En Çok Satan", Dest = d5, Day = 3, Price = 1300 },
                new { Title = "Kyoto Geleneksel Lezzet Rotaları", Desc = "Sushi, Ramen ve Matcha çayının anavatanında sokak pazarlarını uzman rehberle gezeceğiniz lezzet haritası.", Cover = "1542051812-a72ba8980b43", Badge = "Yeni", Dest = d5, Day = 4, Price = 1450 },

                // MALDIVLER
                new { Title = "Maldivler Su Üstü Villa Konaklaması", Desc = "Gözünüzü turkuaz sulara açacağınız, cam tabanlı lüks su üstü villalarında kişiye özel hizmet veren rüya tatil.", Cover = "1573245465955-46c64b6ff13d", Badge = "Premium", Dest = d6, Day = 7, Price = 4500 },
                new { Title = "Maldivler Tüplü Dalış ve Resifler", Desc = "Dünyanın en zengin mercan resiflerinde, renkli tropikal balıklar ve deniz kaplumbağalarıyla birlikte yüzeceksiniz.", Cover = "1500930287596-c14b5d3e1cb9", Badge = "Macera", Dest = d6, Day = 5, Price = 3200 },
                new { Title = "Maldivler Özel Tekne ile Ada Avı", Desc = "Sadece size tahsis edilmiş tekneyle ıssız adalara yolculuk, kumsalda size özel hazırlanan barbekü partisi.", Cover = "1590523277543-a9b6cb21c277", Badge = "Lüks", Dest = d6, Day = 6, Price = 3800 },
                new { Title = "Maldivler Deniz Uçağı Transferli Tatil", Desc = "Başkentten adanıza deniz uçağıyla geçerken Hint Okyanusu'nun muhteşem atol manzaralarına şahit olun.", Cover = "1514282401089-ce3a571e2372", Badge = "Fırsat", Dest = d6, Day = 5, Price = 2900 },
                new { Title = "Maldivler Aile Boyu Huzur Paketi", Desc = "Çocuk kulüpleri ve geniş sahil villaları ile ailenizle stresten uzak, bembeyaz kumlarda bir dinlenme fırsatı.", Cover = "1573245465955-46c64b6ff13d", Badge = "Aile", Dest = d6, Day = 7, Price = 4100 }
            };

            var tours = new List<Tour>();
            var plans = new List<TourPlan>();
            var images = new List<TourImage>();
            var reviews = new List<Review>();

            string[] uniqueReviews = {
                "Şu ana kadar katıldığım en profesyonel turdu. Özellikle otel seçimleri tek kelimeyle muazzamdı.",
                "Program o kadar güzel dengelenmişti ki hem çok yer gördük hem de hiç yorulmadık. Teşekkürler!",
                "Fotoğraflarda göründüğünden çok daha büyüleyici bir atmosfere sahip. Rehberimizin ilgisi harikaydı.",
                "Eşimle yıldönümümüz için tercih ettik. Bize hazırladıkları sürpriz oda süslemesi için minnettarız.",
                "Yemekler, ulaşım, rehberlik... Her şey kusursuzdu. Kesinlikle verdiğimiz ücrete sonuna kadar değdi.",
                "Manzaralar karşısında nutkum tutuldu. Tur boyunca her anımız dolu dolu geçti, çok memnun kaldım.",
                "Ekip çok cana yakındı, turdaki diğer misafirlerle de çok güzel kaynaştık. Unutulmaz bir anı oldu.",
                "Bölgenin tarihi dokusunu rehberimiz adeta yaşayarak anlattı. Kültürel anlamda çok doyurucuydu.",
                "İlk gün havalimanı transferinden son günkü dönüşe kadar hiçbir aksaklık yaşamadık. Güven verici.",
                "Özellikle fotoğraf çekimi için götürdükleri gizli lokasyonlar inanılmazdı. Harika kareler yakaladım."
            };
            string[] reviewerNames = { "Oğuzhan Şahin", "Merve Kılıç", "Ahmet Yılmaz", "Cansu Aydın", "Kemal Tekin", "Zeynep Çelik", "Burak Kaya", "Elif Demir", "Emre Can", "Seda Nur" };

            // Fotoğraf çeşitliliği için galeri havuzu (Ortak kaliteli manzara fotoları)
            string[] galPool = { "1476514525535-07fb3b4ed5f1", "1499856871958-5b9627545d1a", "1502602898657-3e91760cbb34", "1511739001486-6bfe10ce785f", "1531572753322-ad0110ce36f1", "1518548419970-58e3b4079ab2", "1641128324972-af3212f0f6bd", "1493976040374-85c8e12f0c0e", "1514282401089-ce3a571e2372" };

            foreach (var tDef in tourDefinitions)
            {
                string tourId = ObjectId.GenerateNewId().ToString();

                string titleTr = tDef.Title;
                string titleEn = tDef.Title.Replace("Turu", "Tour").Replace("Tur", "Tour").Replace("Gezisi", "Trip").Replace("Keşfi", "Discovery");
                string titleDe = tDef.Title.Replace("Turu", "Tour").Replace("Tur", "Tour").Replace("Gezisi", "Reise").Replace("Keşfi", "Entdeckung");
                
                string descTr = tDef.Desc + "<br/><br/>Bu unutulmaz yolculukta sıradan bir turist olmanın ötesine geçecek, bölgenin ruhunu tam anlamıyla hissedeceksiniz. Özenle seçilmiş lüks konaklama seçeneklerimiz, deneyimli rehberlerimiz ve sadece size özel hazırlanmış sürpriz aktivitelerle tatiliniz bir rüyaya dönüşecek. Gittiğiniz her sokakta yeni bir hikaye keşfederken, yöresel mutfağın en seçkin örneklerini tadacaksınız.<br/><br/>Seyahatiniz boyunca konforunuz için her detay düşünüldü. Erken rezervasyon fırsatlarını kaçırmayın ve hayallerinizdeki bu muazzam maceraya ilk adımı hemen atın!";
                string descEn = "Discover the unique beauties of this region with our professional guides. This premium tour includes luxury accommodation, local delicacies, and historical insights.<br/><br/>On this unforgettable journey, you will go beyond being an ordinary tourist and truly feel the soul of the region. Your vacation will turn into a dream with our carefully selected luxury accommodation options, experienced guides, and surprise activities tailored just for you. While discovering a new story in every street you visit, you will taste the most exquisite examples of local cuisine.<br/><br/>Every detail has been considered for your comfort throughout your trip. Do not miss the early booking opportunities and take the first step towards this tremendous adventure of your dreams right now!";
                string descDe = "Entdecken Sie die einzigartigen Schönheiten dieser Region mit unseren professionellen Guides. Diese Premium-Tour bietet Luxusunterkünfte, lokale Köstlichkeiten und historische Einblicke.<br/><br/>Auf dieser unvergesslichen Reise werden Sie mehr als ein gewöhnlicher Tourist sein und die Seele der Region wirklich spüren. Ihr Urlaub wird zu einem Traum durch unsere sorgfältig ausgewählten Luxusunterkünfte, erfahrene Guides und eigens für Sie maßgeschneiderte Überraschungsaktivitäten. Während Sie in jeder Straße eine neue Geschichte entdecken, probieren Sie die exquisitesten Beispiele der lokalen Küche.<br/><br/>Für Ihren Komfort während der gesamten Reise wurde an jedes Detail gedacht. Verpassen Sie nicht die Frühbucherangebote und machen Sie jetzt den ersten Schritt zu diesem gewaltigen Abenteuer Ihrer Träume!";

                var tour = new Tour
                {
                    TourID = tourId,
                    Title = titleTr,
                    Title_EN = titleEn,
                    Title_DE = titleDe,
                    Description = descTr,
                    Description_EN = descEn,
                    Description_DE = descDe,
                    CoverImageUrl = $"https://images.unsplash.com/photo-{tDef.Cover}?w=800&q=80",
                    Badge = tDef.Badge,
                    DayCount = tDef.Day,
                    Capacity = rnd.Next(10, 40),
                    Price = tDef.Price,
                    IsStatus = true,
                    Location = $"{tDef.Dest.CityName}, {tDef.Dest.CountryName}",
                    DestinationID = tDef.Dest.DestinationID,
                    CategoryID = categories[rnd.Next(categories.Count)].CategoryId,
                    MapLocationImageUrl = "https://images.unsplash.com/photo-1524661135-423995f22d0b?w=800&q=80" // Standart harita görseli
                };
                tours.Add(tour);

                // 4 Adet Rastgele Çalışan Galeri Görseli
                for (int j = 0; j < 4; j++)
                {
                    images.Add(new TourImage
                    {
                        TourImageID = ObjectId.GenerateNewId().ToString(),
                        TourID = tourId,
                        ImageUrl = $"https://images.unsplash.com/photo-{galPool[rnd.Next(galPool.Length)]}?w=800&q=80"
                    });
                }

                // Tur Planı
                for (int d = 1; d <= tDef.Day; d++)
                {
                    string pTitle = d == 1 ? "Varış ve Otele Yerleşme" : (d == tDef.Day ? "Alışveriş ve Dönüş" : "Bölge Keşfi ve Aktiviteler");
                    string pDesc = d == 1 ? "Havalimanında VIP araçlarımızla karşılama ve otelimize transfer." : (d == tDef.Day ? "Öğleden sonra havalimanına transfer ve tur sonu." : "Tam gün rehber eşliğinde tarihi ve doğal güzelliklerin gezilmesi.");
                    
                    plans.Add(new TourPlan
                    {
                        TourPlanID = ObjectId.GenerateNewId().ToString(),
                        TourID = tourId,
                        DayNumber = d,
                        Title = $"{d}. Gün: {pTitle}",
                        Description = pDesc
                    });
                }

                // Yorumlar (3 ile 5 arası tamamen benzersiz yorum)
                int reviewCount = rnd.Next(3, 6);
                for (int r = 0; r < reviewCount; r++)
                {
                    reviews.Add(new Review
                    {
                        ReviewId = ObjectId.GenerateNewId().ToString(),
                        TourId = tourId,
                        NameSurname = reviewerNames[rnd.Next(reviewerNames.Length)],
                        Detail = uniqueReviews[rnd.Next(uniqueReviews.Length)],
                        GuideRating = rnd.Next(4, 6),
                        AccommodationRating = rnd.Next(4, 6),
                        TransportRating = rnd.Next(4, 6),
                        ComfortRating = rnd.Next(4, 6),
                        ReviewDate = DateTime.Now.AddDays(-rnd.Next(1, 100)),
                        Status = true
                    });
                }
            }

            // Toplu olarak DB'ye yaz
            tourCol.InsertMany(tours);
            planCol.InsertMany(plans);
            imgCol.InsertMany(images);
            reviewCol.InsertMany(reviews);
        }
    }
}