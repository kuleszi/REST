Dokumentacja techniczna projektu – System Aukcyjny REST


1.  Autorzy

	Aleksandra Kulesza
	Karolina Mazur
	Patryk Procyk
	Katarzyna Tyc


2. Opis projektu
	Projekt przedstawia system aukcji internetowych oparty o architekturê REST. Aplikacja umo¿liwia rejestracjê u¿ytkowników, logowanie, zarz¹dzanie aukcjami oraz sk³adanie ofert licytacyjnych.
	System zosta³ zaimplementowany w technologii ASP.NET Core Web API zgodnie z zasadami architektury warstwowej.

3. Wykorzystane technologie
	ASP.NET Core Web API (.NET 8)
	Entity Framework Core
	SQLite
	JWT (JSON Web Token)
	Swagger / OpenAPI
	Docker
	xUnit (testy jednostkowe)
	Git

4. Logika biznesowa

Tworzenie aukcji
	Podczas tworzenia aukcji system:
	sprawdza czy w³aœciciel istnieje,
	sprawdza poprawnoœæ dat,
	ustawia status aukcji jako Active,
	ustawia aktualn¹ najwy¿sz¹ ofertê równ¹ cenie wywo³awczej.

Sk³adanie ofert
	Podczas sk³adania oferty system:
	sprawdza czy aukcja istnieje,
	sprawdza czy u¿ytkownik istnieje,
	sprawdza czy aukcja jest aktywna,
	sprawdza czy nie up³yn¹³ termin zakoñczenia aukcji,
	sprawdza czy nowa oferta jest wy¿sza od aktualnej najwy¿szej oferty,
	zapisuje ofertê,
	aktualizuje pole CurrentHighestBid.

Aktualizacja aukcji
	System nie pozwala:
	ustawiæ daty zakoñczenia wczeœniejszej ni¿ data rozpoczêcia,
	ustawiæ ceny wywo³awczej wy¿szej od aktualnej najwy¿szej oferty, jeœli istniej¹ ju¿ oferty.


5. Architektura systemu
	Projekt zosta³ zrealizowany z wykorzystaniem architektury warstwowej:
	Controller -> Service -> Database -> SQLite


Warstwa Controller
	Odpowiada za obs³ugê ¿¹dañ HTTP oraz zwracanie odpowiedzi klientowi.

Warstwa Service
	Zawiera logikê biznesow¹ aplikacji, np. logowanie u¿ytkowników, generowanie tokenów JWT, zarz¹dzanie aukcjami i ofertami.

Warstwa Database
	Odpowiada za dostêp do danych i komunikacjê z baz¹ danych przy u¿yciu Entity Framework Core oraz klasy AppDbContext. 
6. Model danych


7. Relacje:
System wykorzystuje nastêpuj¹ce relacje:
	Jeden u¿ytkownik mo¿e utworzyæ wiele aukcji.
	Jeden u¿ytkownik mo¿e z³o¿yæ wiele ofert.
	Jedna aukcja mo¿e posiadaæ wiele ofert.
	Jedna oferta nale¿y do jednego u¿ytkownika			.
	Jedna oferta nale¿y do jednej aukcji.

8. Endpointy REST API
	Autoryzacja
	Rejestracja u¿ytkownika
	POST /auth/register
	Tworzy nowe konto u¿ytkownika.
	Przyk³adowe ¿¹danie:
	{
	  "name": "Jan Kowalski",
	  "email": "jan@test.pl",
	  "password": "Haslo123!"
	}
	OdpowiedŸ:
	{
	  "token": "jwt_token"
	}
	Kody odpowiedzi:
	200 OK
	400 Bad Request

	Logowanie u¿ytkownika
	POST /auth/login
	Loguje u¿ytkownika i zwraca token JWT.
	Przyk³adowe ¿¹danie:
	{
	  "email": "jan@test.pl",
	  "password": "Haslo123!"
	}
	OdpowiedŸ:
	{
	  "token": "jwt_token"
	}
	Kody odpowiedzi:
	200 OK
	401 Unauthorized

	U¿ytkownicy
	Wszystkie endpointy wymagaj¹ tokenu JWT.
	Dodanie u¿ytkownika
	POST /users
	Przyk³adowe ¿¹danie:
	{
	  "name": "Jan Kowalski",
	  "email": "jan@test.pl"
	}
	Kody odpowiedzi:
	201 Created
	400 Bad Request

	Pobranie listy u¿ytkowników
	GET /users
	Zwraca wszystkich u¿ytkowników.
	Kody odpowiedzi:
	200 OK

	Pobranie u¿ytkownika
	GET /users/{id}
	Przyk³ad:
	GET /users/8e1d5a6f-1234-5678-9999-123456789abc
	Kody odpowiedzi:
	200 OK
	404 Not Found

	Aktualizacja u¿ytkownika
	PUT /users/{id}
	Kody odpowiedzi:
	204 No Content
	404 Not Found

	Usuniêcie u¿ytkownika
	DELETE /users/{id}
	Kody odpowiedzi:
	204 No Content
	404 Not Found

	Aukcje
	Wszystkie endpointy wymagaj¹ tokenu JWT.
	Utworzenie aukcji
	POST /auctions
	Przyk³adowe ¿¹danie:
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
	
	Zwraca listê aukcji dostêpnych w systemie.

	Endpoint obs³uguje:
	filtrowanie wyników,
	sortowanie wyników,
	paginacjê wyników.

	Przyk³ady:
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

	Usuniêcie aukcji
	DELETE /auctions/{id}
	Kody odpowiedzi:
	204 No Content
	404 Not Found

	Licytacje
	Z³o¿enie oferty
	POST /auctions/{id}/bids
	Przyk³adowe ¿¹danie:
	{
	  "userId": "GUID",
	  "price": 3500
	}
	System sprawdza:
	czy u¿ytkownik istnieje,
	czy aukcja istnieje,
	czy aukcja jest aktywna,
	czy nie zosta³a zakoñczona,
	czy oferta jest wy¿sza od aktualnej najwy¿szej oferty.
	Kody odpowiedzi:
	200 OK
	400 Bad Request
	404 Not Found


9. Autoryzacja
	System wykorzystuje mechanizm JWT (JSON Web Token) do uwierzytelniania u¿ytkowników.

	Po poprawnym zalogowaniu u¿ytkownik otrzymuje token JWT generowany przez serwer. 
	Token nale¿y do³¹czaæ do ka¿dego ¿¹dania kierowanego do chronionych endpointów w nag³ówku Authorization. Dziêki temu serwer mo¿e zweryfikowaæ to¿samoœæ u¿ytkownika bez koniecznoœci ponownego przesy³ania loginu i has³a. Endpointy zarz¹dzania u¿ytkownikami oraz aukcjami s¹ zabezpieczone za pomoc¹ atrybutu Authorize. 

10. Walidacja danych
	Projekt wykorzystuje mechanizm Data Annotations do walidacji danych wejœciowych.
	Przyk³adowe ograniczenia:

	U¿ytkownik
	Name – wymagane, maksymalnie 100 znaków
	Email – wymagane, poprawny format adresu e-mail, maksymalnie 200 znaków

	Aukcja
	ItemName – wymagane, maksymalnie 200 znaków
	Description – wymagane, maksymalnie 5000 znaków
	Category – wymagane, maksymalnie 100 znaków
	StartingPrice – wartoœæ od 0.01 do 999999999
	StartDateUtc – wymagane
	EndDateUtc – wymagane

	Oferta
	UserId – wymagane
	Price – wartoœæ od 0.01 do 999999999


11. Obs³uga b³êdów
	System zwraca standardowe kody HTTP:
	200 OK
	201 Created
	204 No Content
	400 Bad Request
	401 Unauthorized
	404 Not Found	
	500 Internal Server Error

12. Testy jednostkowe
	W projekcie zaimplementowano testy jednostkowe przy u¿yciu frameworka xUnit.

	UnitTestAuth
	Testowane scenariusze:
	poprawna rejestracja u¿ytkownika,
	rejestracja u¿ytkownika z istniej¹cym adresem e-mail,
	poprawne logowanie u¿ytkownika.

	UnitTestUser
	Testowane scenariusze:
	pobieranie u¿ytkownika po identyfikatorze.

	UnitTestAuction
	Testowane scenariusze:
	poprawne tworzenie aukcji,
	walidacja dat aukcji,
	obs³uga niepoprawnych danych wejœciowych.


13. Repozytorium
	Link do repozytorium:
	https://github.com/kuleszi/REST.git


