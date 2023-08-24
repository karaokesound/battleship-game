# Battleship Game
Gra Battleship Game jest podzielona na 3 mniejsze projekty:
- BattleshipGame.API
Projekt ten implementuje wzorzec MVC, znajdują się w nim repozytoria (IField, IGame, IPlayer), które służą do komunikacji z bazą danych oraz serwisy dla poszczególnych kontrolerów.
- BattleshipGame.Data
Projekt, który implementuje Entity Framework i określa strukturę bazy danych. Znajdują się tu encje, Dbcontext oraz konfiguracje dla poszczególnych encji (fluent API).
- BattleshipGame.Logic
Projekt, który przechowuje logikę generowania gry, czyli m.in. tworzenie planszy, losowanie koordynatów poszczególnych statków, etc. Zawiera 2 serwisy - jeden do walidacji danych, drugi do generowania poszczególnych elementów gry oraz folder z modelami.
## Projekt
W projekcie wykorzystano
- ASP.NET Core v.6.0
- Entity Framework v6.0.1
- SQLite Db Provider v6.0.1
- Git Flow
 

## Instrukcja 
### Konfiguracja
Przy pierwszej kompilacji programu, baza danych powinna automatycznie się utworzyć, wraz z kilkoma domyślnie wprowadzonymi graczami. Jeśli tak się nie stanie, to należy utworzyć bazę danych ręcznie za pomocą komendy:
##### Package Manager Console
- update-database
##### CLI
 - dotnet ef database update
 
 Należy się upewnić, że robimy to z poziomu projektu w którym znajduje się Entity Framework, czyli w tym przypadku **BattleshipGame.Data**.
 
 Jeśli wystąpi przypadek, że nie mamy graczy w bazie, należy dodać ręcznie (POST) **minimum** 2 za pomocą API.

## Gra
Gra daje także możliwość usuwania gracza (DELETE), modyfikowania (PUT) i częściowego modyfikowania (PATCH).
Przed wykonaniem jakiegokolwiek requestu dotyczącego Playera, musimy upewnić się, że ustawiliśmy obsługiwany format na **application/json**.

Gra jest obsługiwana przez API. Poszczególne etapy gry wyglądają następująco:
1. POST /api/game/start/{key}/{playerName}
Request, który rozpoczyna nową grę. Należy wpisać 1 jako key i podać nazwę użytkownika, z którą chcemy dołączyć do gry [gracz musi istnieć w bazie].
2. GET /api/game/board/{playerName}
Request, który odpowiada za wyświetlenie planszy. Możemy sprawdzić wygenerowane statki i ich położenie.
3. PATCH /api/game/shoot/{playerName}
 Request, który odpowiada za wykonywanie strzałów na planszę przeciwnika. Można wprowadzić max. 3 pary koordynatów w formacie 0,1 0,2 4,2 etc.
4. PATCH /api/game/shoot/opponent
Request, który odpowiada za oddawanie strzałów przez komputer na planszę gracza.

Gra kończy się w momencie, gdy któryś z graczy zestrzeli wszystkie 12 statków przeciwnika.

## Założenia
- Gra obsługuje wyłącznie dwóch graczy i nie przechowuje większej ilości gier.
- Gra generuje 12 statków
- Gra pozwala na oddawanie jednocześnie 3 strzałów
- Celny strzał pozwala na wykonanie kolejnego strzału w tej samej kolejce

### Postman
Grę można testować również z poziomu Postmana. Gotowy plik znajduje się w repozytorium na Githubie.





