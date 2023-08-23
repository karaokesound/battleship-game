namespace BattleshipGame.API.Services
{
    public interface IMessageService
    {
        string PlayerNotFoundMessage();

        string UserCreatingError();

        string Delete();

        string StartGameError();

        string StartGameSuccess(string opponent);

        string GameBoardNotFound();

        string ShootError(string opponent, string key);

        string ShotSuccess(int quantity, List<string> hitShipsCoords);

        string ShotMissed();
    }
}
