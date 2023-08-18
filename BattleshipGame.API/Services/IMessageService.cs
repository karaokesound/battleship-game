namespace BattleshipGame.API.Services
{
    public interface IMessageService
    {
        string PlayerNotFoundMessage();

        string UserCreatingError();

        string Delete();

        string GameBoardNotFound();

        string StartGameError();
    }
}
