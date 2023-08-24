# Battleship Game
Battleship Game jest podzielona na 3 projekty:
- BattleshipGame.API
Projekt ten implementuje wzorzec MVC, dodatkowo znajdują się w nim repozytoria (IField, IGame, IPlayer), które służą do komunikacji z bazą danych oraz serwis IMessageService, wykorzystywany do zwracania tekstowych wiadomości dla poszczególnych status code'ów.
- BattleshipGame.Data
Projekt, który implementuje Entity Framework i określa strukturę bazy danych. Znajdują się tu encje, dbcontext oraz konfiguracje dla poszczególnych encji (fluent API).
- BattleshipGame.Logic
Projekt, który przedstawia całą logikę gry, czyli m.in. tworzenie planszy, losowanie koordynatów poszczególnych statków, etc. Zawiera 2 serwisy - jeden do walidacji danych, drugi do generowania poszczególnych elementów gry oraz modele.
## Projekt
W projekcie wykorzystano
- ASP.NET Core v.6.0
- Entity Framework v6.0.1
- SQLite Db Provider v6.0.1
- Git Flow
 

## Instrukcja 
### Konfiguracja
Przy pierwszej kompilacji programu, baza danych powinna automatycznie się utworzyć, wraz z kilkoma domyślnie wprowadzonymi graczami. Jeśli by się tak nie stało, to należy utworzyć bazę danych ręcznie za pomocą komendy:
##### Package Manager Console
- update-database
##### CLI
 - dotnet ef database update
 
 Należy się upewnić, że robimy to z poziomu projektu w którym znajduje się Entity Framework, czyli w tym przypadku **BattleshipGame.Data**.
 
 Jeśli wystąpi przypadek, że nie mamy graczy w bazie, należy dodać ręcznie (POST) **minimum** 2 za pomocą API.

## Gra
Gra daje także możliwość usuwania gracza (DELETE), modyfikowania (PUT) i częściowego modyfikowania (PATCH).
Przed wykonaniem jakiegokolwiek requestu, musimy upewnić się, że ustawiliśmy obsługiwany format na **application/json**.

Przed rozpoczęciem gry możemy:
1. osobno wygenerować planszę dla 2 graczy
/api/game/{playerName}
2. wyświetlić wygenerowane plansze
/api/game/board/{playerName}

Gra jest obsługiwana przez API. Poszczególne etapy gry wyglądają następująco:
1. POST /api/game/{key}/{playerName}
Request, który rozpoczyna nową grę. Należy wpisać 1 jako key i podać nazwę użytkownika, z którą chcemy dołączyć do gry [gracz musi istnieć w bazie].
2. POST /api/game/{playerName}/shoot
Request, który odpowiada za wykonywanie strzałów w planszę przeciwnika. Można podać max. 5 par koordynatów w formacie 0,1 0,2 4,2 3,1 etc.
3. ...

### Postman
Grę można testować również z poziomu Postmana. Gotowy plik znajduje się w repozytorium na Githubie.





