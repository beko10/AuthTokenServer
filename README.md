# AuthTokenServer

Bu proje, JWT tabanlı kimlik doğrulama ve token yönetimi sağlayan bir .NET Core Web API çözümüdür. N katmanlı mimari ile tasarlanmış olup, güvenli token üretimi, kullanıcı yönetimi ve API koruması özellikleri sunmaktadır.

## 📋 İçindekiler
- [Proje Yapısı](#proje-yapısı)
- [Özellikler](#özellikler)
- [Teknolojiler](#teknolojiler)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [API Endpoints](#api-endpoints)
- [Yapılandırma](#yapılandırma)
- [Test](#test)

## 🏗️ Proje Yapısı

Proje, **N-Tier Architecture** (Çok Katmanlı Mimari) desenini kullanarak organize edilmiştir:

```
AuthTokenServer/
├── AuthTokenServer.API/                    # Presentation Layer (API Katmanı)
│   ├── Controllers/                        # API Controller'lar
│   ├── Program.cs                          # Uygulama başlangıç noktası
│   └── appsettings.json                    # Konfigürasyon dosyası
├── AuthTokenServer.BusinessLayer/          # Business Logic Layer (İş Mantığı Katmanı)
│   ├── Abstract/                           # Interface'ler
│   ├── Concrete/                           # İş mantığı implementasyonları
│   └── Mapping/                           # Object mapping konfigürasyonları
├── AuthTokenServer.DataAccessLayer/        # Data Access Layer (Veri Erişim Katmanı)
│   ├── Abstract/                           # Repository interface'leri
│   ├── Concrete/                           # Repository implementasyonları
│   ├── Context/                           # Entity Framework DbContext
│   └── Migrations/                        # Veritabanı migrasyonları
├── AuthTokenServer.EntityLayer/            # Entity Layer (Varlık Katmanı)
│   ├── Entities/                          # Veritabanı varlıkları
│   └── DTOs/                              # Data Transfer Objects
├── AuthTokenServer.CoreLayer/              # Core Layer (Çekirdek Katman)
│   ├── Configuration/                      # Token ve sistem konfigürasyonları
│   ├── Extensions/                        # Extension metodları
│   └── Utilities/                         # Yardımcı sınıflar
└── MiniApp.API[1-3]/                      # Test uygulamaları
```

## ✨ Özellikler

### Kimlik Doğrulama & Yetkilendirme
- 🔐 JWT (JSON Web Token) tabanlı kimlik doğrulama
- 🔄 Refresh Token desteği
- 👤 Kullanıcı kaydı ve giriş işlemleri
- 🏢 Client Credentials flow desteği
- 🚫 Token iptal etme (Revoke) işlemi

### Güvenlik
- ✅ ASP.NET Core Identity entegrasyonu
- 🛡️ Token tabanlı API koruması
- 🔑 Güvenli şifre politikaları
- 📱 Multiple audience desteği

### Mimari & Yapı
- 🏗️ N-Tier Architecture (Çok Katmanlı Mimari)
- 📊 Entity Framework Core ile ORM
- 🔄 Unit of Work pattern
- 📦 Dependency Injection
- 🗃️ SQL Server veritabanı desteği

### API Dokümantasyonu
- 📚 Swagger/OpenAPI entegrasyonu
- 🔍 Detaylı API dokümantasyonu
- 🧪 Swagger UI ile test imkanı

## 🛠️ Teknolojiler

- **.NET 8.0** - Framework
- **ASP.NET Core Web API** - API geliştirme
- **Entity Framework Core** - ORM
- **SQL Server** - Veritabanı
- **ASP.NET Core Identity** - Kimlik yönetimi
- **JWT (JSON Web Tokens)** - Token tabanlı kimlik doğrulama
- **AutoMapper** - Object mapping
- **Swagger/OpenAPI** - API dokümantasyonu

## 📥 Kurulum

### Gereksinimler
- .NET 8.0 SDK veya üzeri
- SQL Server (LocalDB veya tam sürüm)
- Visual Studio 2022 veya Visual Studio Code

### Adımlar

1. **Projeyi klonlayın:**
```bash
git clone [repository-url]
cd AuthTokenServer
```

2. **Bağımlılıkları yükleyin:**
```bash
dotnet restore
```

3. **Veritabanı connection string'ini güncelleyin:**
`AuthTokenServer.API/appsettings.json` dosyasındaki connection string'i kendi SQL Server bilgilerinizle güncelleyin:
```json
{
  "ConnectionStrings": {
    "SqlServerConnection": "Data Source=SUNUCU_ADI;Initial Catalog=AuthTokenServerDb;Integrated Security=True;..."
  }
}
```

4. **Veritabanını oluşturun:**
```bash
cd AuthTokenServer.DataAccessLayer
dotnet ef database update
```

5. **Projeyi çalıştırın:**
```bash
cd AuthTokenServer.API
dotnet run
```

## 🚀 Kullanım

### 1. Kullanıcı Kaydı
```http
POST /api/User/CreateUser
Content-Type: application/json

{
  "userName": "testuser",
  "email": "test@example.com",
  "password": "1234"
}
```

### 2. Token Alma
```http
POST /api/Auth/CreateToken
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "1234"
}
```

### 3. Korumalı Endpoint'e Erişim
```http
GET /api/Product
Authorization: Bearer {access_token}
```

### 4. Refresh Token ile Yeni Token Alma
```http
POST /api/Auth/CreateTokenByRefreshToken
Content-Type: application/json

{
  "refreshToken": "your_refresh_token_here"
}
```

## 📡 API Endpoints

### Authentication
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | `/api/Auth/CreateToken` | Email/şifre ile token oluşturma |
| POST | `/api/Auth/CreateTokenByRefreshToken` | Refresh token ile yeni token alma |
| POST | `/api/Auth/RevokeRefreshToken` | Refresh token iptal etme |
| POST | `/api/Auth/CreateTokenByClient` | Client credentials ile token alma |

### User Management
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | `/api/User/CreateUser` | Yeni kullanıcı kaydı |
| GET | `/api/User/GetUser` | Kullanıcı bilgilerini getirme |

### Products (Örnek korumalı endpoint)
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/Product` | Tüm ürünleri listele |
| POST | `/api/Product` | Yeni ürün ekle |

## ⚙️ Yapılandırma

### Token Ayarları
`appsettings.json` dosyasında token ayarlarını yapılandırabilirsiniz:

```json
{
  "TokenOption": {
    "Audience": ["www.authserver.com", "www.miniapi1.com"],
    "Issuer": "www.authserver.com",
    "AccessTokenExpiration": 5,        // dakika
    "RefreshTokenExpiration": 600,     // dakika
    "SecurityKey": "your-secret-key-here"
  }
}
```

### Client Konfigürasyonu
Farklı client'lar için ayarlar:

```json
{
  "Clients": [
    {
      "Id": "SpaApp",
      "Secret": "secret",
      "Audiences": ["www.miniapi1.com"]
    }
  ]
}
```

## 🧪 Test

### MiniApp Test Uygulamaları
Proje ile birlikte 3 adet test uygulaması gelir:
- **MiniApp.API1** - Port: 5001
- **MiniApp.API2** - Port: 5002  
- **MiniApp.API3** - Port: 5003

Bu uygulamalar, token doğrulama ve API güvenliğini test etmek için kullanılabilir.

### Swagger ile Test
1. Uygulamayı çalıştırın
2. `https://localhost:5000/swagger` adresine gidin
3. "Authorize" butonuna tıklayın
4. Token'ı `Bearer {token}` formatında girin
5. API endpoint'lerini test edin

## 📝 Notlar

- **Güvenlik**: Production ortamında `SecurityKey` değerini güvenli bir şekilde saklayın
- **Veritabanı**: Migration'ları düzenli olarak çalıştırın
- **Loglama**: Production ortamında loglama seviyelerini ayarlayın
- **Performance**: Cache mekanizmaları eklenebilir

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'i push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun



**Not**: Bu README dosyası projenin genel yapısını ve kullanımını açıklamaktadır. Daha detaylı bilgi için kaynak kodları inceleyebilirsiniz.
