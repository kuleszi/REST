Dokumentacja techniczna projektu � System Aukcyjny REST

1.  Autorzy

    Aleksandra Kulesza
    Karolina Mazur
    Patryk Procyk
    Katarzyna Tyc

2.  Opis projektu
    Projekt przedstawia system aukcji internetowych oparty o architektur� REST. Aplikacja umo�liwia rejestracj� u�ytkownik�w, logowanie, zarz�dzanie aukcjami oraz sk�adanie ofert licytacyjnych.
    System został zaimplementowany w technologii ASP.NET Core Web API z dodatkową warstwą widoków MVC, co umożliwia przeglądanie aukcji bezpośrednio w przeglądarce.

3.  Wykorzystane technologie
    ASP.NET Core Web API (.NET 8)
    Entity Framework Core
    SQLite
    JWT (JSON Web Token)
    Swagger / OpenAPI
    Docker
    xUnit (testy jednostkowe)
    Git

4.  Logika biznesowa

Tworzenie aukcji
Podczas tworzenia aukcji system:
sprawdza czy w�a�ciciel istnieje,
sprawdza poprawno�� dat,
ustawia status aukcji jako Active,
ustawia aktualn� najwy�sz� ofert� r�wn� cenie wywo�awczej.

Sk�adanie ofert
Podczas sk�adania oferty system:
sprawdza czy aukcja istnieje,
sprawdza czy u�ytkownik istnieje,
sprawdza czy aukcja jest aktywna,
sprawdza czy nie up�yn�� termin zako�czenia aukcji,
sprawdza czy nowa oferta jest wy�sza od aktualnej najwy�szej oferty,
zapisuje ofert�,
aktualizuje pole CurrentHighestBid.

Aktualizacja aukcji
System nie pozwala:
ustawi� daty zako�czenia wcze�niejszej ni� data rozpocz�cia,
ustawi� ceny wywo�awczej wy�szej od aktualnej najwy�szej oferty, je�li istniej� ju� oferty.

5. Architektura systemu
   Projekt zosta� zrealizowany z wykorzystaniem architektury warstwowej:
   Controller -> Service -> Database -> SQLite oraz View (MVC).

Warstwa Controller
Odpowiada za obs�ug� ��da� HTTP oraz zwracanie odpowiedzi klientowi.

Warstwa Service
Zawiera logik� biznesow� aplikacji, np. logowanie u�ytkownik�w, generowanie token�w JWT, zarz�dzanie aukcjami i ofertami.

Warstwa Database
Odpowiada za dost�p do danych i komunikacj� z baz� danych przy u�yciu Entity Framework Core oraz klasy AppDbContext. 6. Model danych

7. Relacje:
   System wykorzystuje nast�puj�ce relacje:
   Jeden u�ytkownik mo�e utworzy� wiele aukcji.
   Jeden u�ytkownik mo�e z�o�y� wiele ofert.
   Jedna aukcja mo�e posiada� wiele ofert.
   Jedna oferta nale�y do jednego u�ytkownika .
   Jedna oferta nale�y do jednej aukcji.

8. Endpointy REST API
   Autoryzacja
   Rejestracja u�ytkownika
   POST /auth/register
   Tworzy nowe konto u�ytkownika.
   Przyk�adowe ��danie:
   {
   "name": "Jan Kowalski",
   "email": "jan@test.pl",
   "password": "Haslo123!"
   }
   Odpowied�:
   {
   "token": "jwt_token"
   }
   Kody odpowiedzi:
   200 OK
   400 Bad Request

   Logowanie u�ytkownika
   POST /auth/login
   Loguje u�ytkownika i zwraca token JWT.
   Przyk�adowe ��danie:
   {
   "email": "jan@test.pl",
   "password": "Haslo123!"
   }
   Odpowied�:
   {
   "token": "jwt_token"
   }
   Kody odpowiedzi:
   200 OK
   401 Unauthorized

   U�ytkownicy
   Wszystkie endpointy wymagaj� tokenu JWT.
   Dodanie u�ytkownika
   POST /users
   Przyk�adowe ��danie:
   {
   "name": "Jan Kowalski",
   "email": "jan@test.pl"
   }
   Kody odpowiedzi:
   201 Created
   400 Bad Request

   Pobranie listy u�ytkownik�w
   GET /users
   Zwraca wszystkich u�ytkownik�w.
   Kody odpowiedzi:
   200 OK

   Pobranie u�ytkownika
   GET /users/{id}
   Przyk�ad:
   GET /users/8e1d5a6f-1234-5678-9999-123456789abc
   Kody odpowiedzi:
   200 OK
   404 Not Found

   Aktualizacja u�ytkownika
   PUT /users/{id}
   Kody odpowiedzi:
   204 No Content
   404 Not Found

   Usuni�cie u�ytkownika
   DELETE /users/{id}
   Kody odpowiedzi:
   204 No Content
   404 Not Found

   Aukcje
   Wszystkie endpointy wymagaj� tokenu JWT.
   Utworzenie aukcji
   POST /auctions
   Przyk�adowe ��danie:
   {
   "itemName": "iPhone 15",
   "description": "Telefon w bardzo dobrym stanie",
   "category": "Elektronika",
   "startingPrice": 3000,
   "startDateUtc": "2025-06-01T10:00:00Z",
   "endDateUtc": "2025-06-08T10:00:00Z",
   "ownerId": "GUID"
   }
   Kody odpowiedzi:
   201 Created
   400 Bad Request

   Pobranie wszystkich aukcji
   GET /auctions

   Zwraca list� aukcji dost�pnych w systemie.

   Endpoint obs�uguje:
   filtrowanie wynik�w,
   sortowanie wynik�w,
   paginacj� wynik�w.

   Przyk�ady:
   GET /auctions
   GET /auctions?category=Elektronika
   GET /auctions?status=Active
   Kody odpowiedzi:
   200 OK

   Pobranie aukcji
   GET /auctions/{id}
   Kody odpowiedzi:
   200 OK
   404 Not Found

   Aktualizacja aukcji
   PUT /auctions/{id}
   Kody odpowiedzi:
   204 No Content
   400 Bad Request
   404 Not Found

   Usuni�cie aukcji
   DELETE /auctions/{id}
   Kody odpowiedzi:
   204 No Content
   404 Not Found

   Licytacje
   Z�o�enie oferty
   POST /auctions/{id}/bids
   Przyk�adowe ��danie:
   {
   "userId": "GUID",
   "price": 3500
   }
   System sprawdza:
   czy u�ytkownik istnieje,
   czy aukcja istnieje,
   czy aukcja jest aktywna,
   czy nie zosta�a zako�czona,
   czy oferta jest wy�sza od aktualnej najwy�szej oferty.
   Kody odpowiedzi:
   200 OK
   400 Bad Request
   404 Not Found

   Interfejs użytkownika (Widoki MVC)
   Główny widok (Index): Pod adresem głównym aplikacji (/) dostępna jest lista wszystkich aukcji, wyrenderowana po stronie serwera. Interfejs pozwala na przeglądanie aukcji w czytelnej tabeli wraz z kategoriami i opisami.
   

9. Autoryzacja
   System wykorzystuje mechanizm JWT (JSON Web Token) do uwierzytelniania u�ytkownik�w.

   Po poprawnym zalogowaniu u�ytkownik otrzymuje token JWT generowany przez serwer.
   Token nale�y do��cza� do ka�dego ��dania kierowanego do chronionych endpoint�w w nag��wku Authorization. Dzi�ki temu serwer mo�e zweryfikowa� to�samo�� u�ytkownika bez konieczno�ci ponownego przesy�ania loginu i has�a. Endpointy zarz�dzania u�ytkownikami oraz aukcjami s� zabezpieczone za pomoc� atrybutu Authorize.

10. Walidacja danych
    Projekt wykorzystuje mechanizm Data Annotations do walidacji danych wej�ciowych.
    Przyk�adowe ograniczenia:

    U�ytkownik
    Name � wymagane, maksymalnie 100 znak�w
    Email � wymagane, poprawny format adresu e-mail, maksymalnie 200 znak�w

    Aukcja
    ItemName � wymagane, maksymalnie 200 znak�w
    Description � wymagane, maksymalnie 5000 znak�w
    Category � wymagane, maksymalnie 100 znak�w
    StartingPrice � warto�� od 0.01 do 999999999
    StartDateUtc � wymagane
    EndDateUtc � wymagane

    Oferta
    UserId � wymagane
    Price � warto�� od 0.01 do 999999999

11. Obs�uga b��d�w
    System zwraca standardowe kody HTTP:
    200 OK
    201 Created
    204 No Content
    400 Bad Request
    401 Unauthorized
    404 Not Found
    500 Internal Server Error

12. Testy jednostkowe
    W projekcie zaimplementowano testy jednostkowe przy u�yciu frameworka xUnit.

    UnitTestAuth
    Testowane scenariusze:
    poprawna rejestracja u�ytkownika,
    rejestracja u�ytkownika z istniej�cym adresem e-mail,
    poprawne logowanie u�ytkownika.

    UnitTestUser
    Testowane scenariusze:
    pobieranie u�ytkownika po identyfikatorze.

    UnitTestAuction
    Testowane scenariusze:
    poprawne tworzenie aukcji,
    walidacja dat aukcji,
    obs�uga niepoprawnych danych wej�ciowych.

13. Repozytorium
    Link do repozytorium:
    https://github.com/kuleszi/REST.git

14. Jak uruchomić aplikację

Wymagania: .NET 8.0 SDK.

Konfiguracja: W pliku appsettings.json zdefiniuj klucze JWT (Key, Issuer, Audience).

Uruchomienie: W terminalu w głównym folderze projektu wpisz: dotnet watch

Dostęp do aplikacji:

Widok aukcji: http://localhost:5229/

Panel API (Swagger): http://localhost:5229/swagger

Baza danych: Aplikacja automatycznie przeprowadza migrację (app.db) i wypełnia ją danymi testowymi przy pierwszym uruchomieniu za pomocą klasy SeedData.
