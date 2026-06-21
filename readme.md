# Dokumentacja techniczna projektu — System Aukcyjny REST

### 1. Autorzy

* Aleksandra Kulesza
* Karolina Mazur
* Patryk Procyk
* Katarzyna Tyc

### 2. Opis projektu

Projekt przedstawia system aukcji internetowych oparty o architekturę REST. Aplikacja umożliwia rejestrację użytkowników, logowanie, zarządzanie aukcjami oraz składanie ofert licytacyjnych. System został zaimplementowany w technologii ASP.NET Core Web API z dodatkową warstwą widoków MVC, co umożliwia przeglądanie aukcji bezpośrednio w przeglądarce.

### 3. Wykorzystane technologie

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* SQLite
* JWT (JSON Web Token)
* Swagger / OpenAPI
* Docker
* xUnit (testy jednostkowe)
* Git

### 4. Logika biznesowa

**Tworzenie aukcji**
Podczas tworzenia aukcji system:

* sprawdza czy właściciel istnieje,
* sprawdza poprawność dat,
* ustawia status aukcji jako Active,
* ustawia aktualną najwyższą ofertę równą cenie wywoławczej.

**Składanie ofert**
Podczas składania oferty system:

* sprawdza czy aukcja istnieje,
* sprawdza czy użytkownik istnieje,
* sprawdza czy aukcja jest aktywna,
* sprawdza czy nie upłynął termin zakończenia aukcji,
* sprawdza czy nowa oferta jest wyższa od aktualnej najwyższej oferty,
* zapisuje ofertę,
* aktualizuje pole CurrentHighestBid.

**Aktualizacja aukcji**
System nie pozwala:

* ustawić daty zakończenia wcześniejszej niż data rozpoczęcia,
* ustawić ceny wywoławczej wyższej od aktualnej najwyższej oferty, jeśli istnieją już oferty.

### 5. Architektura systemu

Projekt został zrealizowany z wykorzystaniem architektury warstwowej:
Controller -> Service -> Database -> SQLite oraz View (MVC).

**Warstwa Controller**
Odpowiada za obsługę żądań HTTP oraz zwracanie odpowiedzi klientowi.

**Warstwa Service**
Zawiera logikę biznesową aplikacji, np. logowanie użytkowników, generowanie tokenów JWT, zarządzanie aukcjami i ofertami.

**Warstwa Database**
Odpowiada za dostęp do danych i komunikację z bazą danych przy użyciu Entity Framework Core oraz klasy AppDbContext.

### 6. Model danych

(Sekcja zawiera definicje klas modelu danych)

### 7. Relacje:

System wykorzystuje następujące relacje:

* Jeden użytkownik może utworzyć wiele aukcji.
* Jeden użytkownik może złożyć wiele ofert.
* Jedna aukcja może posiadać wiele ofert.
* Jedna oferta należy do jednego użytkownika.
* Jedna oferta należy do jednej aukcji.

### 8. Endpointy REST API oraz Interfejs użytkownika

**8.1. Interfejs użytkownika (Widoki MVC)**

* **Główny widok (Index):** Pod adresem głównym aplikacji (/) dostępna jest lista wszystkich aukcji, wyrenderowana po stronie serwera. Interfejs pozwala na przeglądanie aukcji w czytelnej tabeli wraz z kategoriami i opisami.

**8.2. REST API (Endpointy)**
*Wszystkie poniższe endpointy zwracają dane w formacie JSON i wymagają (tam gdzie zaznaczono) tokenu JWT.*

**Autoryzacja**

* `POST /auth/register` – Rejestracja nowego użytkownika.
* `POST /auth/login` – Logowanie użytkownika, zwraca token JWT.

**Użytkownicy**

* `POST /users` – Dodawanie użytkownika.
* `GET /users` – Pobranie listy użytkowników.
* `GET /users/{id}` – Pobranie danych konkretnego użytkownika.
* `PUT /users/{id}` – Aktualizacja danych użytkownika.
* `DELETE /users/{id}` – Usunięcie użytkownika.

**Aukcje**

* `POST /auctions` – Utworzenie nowej aukcji.
* `GET /auctions` – Pobranie listy aukcji (obsługuje filtrowanie, sortowanie i paginację).
* `GET /auctions/{id}` – Pobranie szczegółów aukcji.
* `PUT /auctions/{id}` – Aktualizacja aukcji.
* `DELETE /auctions/{id}` – Usunięcie aukcji.

**Licytacje**

* `POST /auctions/{id}/bids` – Złożenie oferty w danej aukcji.

### 9. Autoryzacja

System wykorzystuje mechanizm JWT (JSON Web Token) do uwierzytelniania użytkowników. Po poprawnym zalogowaniu użytkownik otrzymuje token JWT generowany przez serwer. Token należy dołączać do każdego żądania kierowanego do chronionych endpointów w nagłówku Authorization.

### 10. Walidacja danych

Projekt wykorzystuje mechanizm Data Annotations do walidacji danych wejściowych. Przykładowe ograniczenia:

* **Użytkownik:** Name – wymagane, maksymalnie 100 znaków; Email – wymagane, poprawny format, maksymalnie 200 znaków.
* **Aukcja:** ItemName – wymagane, do 200 znaków; Description – wymagane, do 5000 znaków; Category – wymagane, do 100 znaków; StartingPrice – 0.01 do 999999999.
* **Oferta:** UserId – wymagane; Price – 0.01 do 999999999.

### 11. Obsługa błędów

System zwraca standardowe kody HTTP: 200 OK, 201 Created, 204 No Content, 400 Bad Request, 401 Unauthorized, 404 Not Found, 500 Internal Server Error.

### 12. Testy jednostkowe

W projekcie zaimplementowano testy jednostkowe przy użyciu frameworka xUnit:

* **UnitTestAuth:** poprawne rejestrowanie i logowanie.
* **UnitTestUser:** pobieranie użytkownika.
* **UnitTestAuction:** tworzenie aukcji, walidacja dat, obsługa błędnych danych.

### 13. Repozytorium

Link do repozytorium: [https://github.com/kuleszi/REST.git](https://github.com/kuleszi/REST.git)

### 14. Jak uruchomić aplikację

1. **Wymagania:** .NET 8.0 SDK.
2. **Konfiguracja:** W pliku `appsettings.json` zdefiniuj klucze JWT (Key, Issuer, Audience).
3. **Uruchomienie:** W terminalu w głównym folderze projektu wpisz: `dotnet watch`.
4. **Dostęp do aplikacji:**
* Widok aukcji: `http://localhost:5229/`
* Panel API (Swagger): `http://localhost:5229/swagger`


5. **Baza danych:** Aplikacja automatycznie przeprowadza migrację (`app.db`) i wypełnia ją danymi testowymi przy pierwszym uruchomieniu za pomocą klasy SeedData.