# BookShopAppAPI — MVC layihəsinin API versiyası

Bu layihə sənin BookShop MVC layihəndəki funksiyaların ayrıca ASP.NET Core Web API versiyasıdır. MVC layihəsinə reference yoxdur. Razor View, CSS və frontend faylları yoxdur; controller-lər JSON qaytarır.

## Açmaq

Visual Studio-da `BookShopAppAPI.slnx` faylını açın. Solution Explorer-də Controllers, Contracts, Data, Models, Repositories və Services qovluqlarını görəcəksiniz.

## İşə salmaq

```powershell
dotnet restore --configfile NuGet.Config
dotnet run
```

Layihə əlavə quraşdırma tələb etməyən SQLite istifadə edir. Tətbiq ilk açılışda `bookshop.db` bazasını və admin istifadəçisini yaradır.

Admin məlumatları:

```text
username: admin
password: admin123
```

## Əsas endpoint-lər

- `GET /api/books`
- `GET /api/books/{id}`
- `POST /api/books` — Admin
- `PUT /api/books/{id}` — Admin
- `DELETE /api/books/{id}` — Admin
- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/logout`
- `GET /api/cart`
- `POST /api/cart/items`
- `DELETE /api/cart/items/{id}`
- `GET /api/orders`
- `POST /api/orders/checkout`
- `POST /api/orders/{id}/cancel`
- `GET /api/admin/dashboard` — Admin
- `GET /api/admin/orders` — Admin
- `PUT /api/admin/orders/{id}/status` — Admin
- `GET /api/admin/ratings` — Admin

Sorğuları Visual Studio-da `BookShopAppAPI.http` faylı ilə yoxlaya bilərsiniz.

## Kod quruluşu

- `Repositories/Abstract` və `Services/Abstract` interfeysləri saxlayır.
- `Repositories/Concrete` və `Services/Concrete` həmin interfeyslərin implementasiyalarını saxlayır.
- Namespace-lər qovluqlara uyğundur; asılılıqlar `Program.cs` daxilində DI ilə birləşdirilir.
- `Repositories/Base` və `Services/Base` qovluqlarında ümumi repository və service əsasları ayrıca fayllardadır.
- `Contracts/Responses` qovluğunda hər API cavab modeli ayrıca fayldadır.
- `Mappings/ApiMappingProfile.cs` entity və sorğu modellərini cavab modellərinə xəritələyir. Servislər `AutoMapper.IMapper` istifadə edir.
- AutoMapper 16 üçün lisenziya açarı lazımdır. Açarı mənbə koduna yazmadan `AutoMapper__LicenseKey` mühit dəyişəni ilə verə bilərsiniz.

## MVC ilə müqayisə

```text
MVC BookController/Index      -> GET    /api/books
MVC BookController/Detail     -> GET    /api/books/{id}
MVC BookController/Create     -> POST   /api/books
MVC BookController/Edit       -> PUT    /api/books/{id}
MVC BookController/Delete     -> DELETE /api/books/{id}
MVC BookController/Rate       -> POST   /api/books/{id}/rating
MVC AccountController/Login   -> POST   /api/auth/login
MVC AccountController/Register-> POST   /api/auth/register
MVC CartController/Index      -> GET    /api/cart
MVC CartController/Add        -> POST   /api/cart/items
MVC OrderController/Checkout  -> POST   /api/orders/checkout
MVC OrderController/MyOrders  -> GET    /api/orders
MVC AdminController/Index     -> GET    /api/admin/dashboard
```

MVC controller `View(model)` qaytarırdı. API controller isə `Ok(data)`, `Created(...)`, `NotFound()` və başqa HTTP cavabları qaytarır.
