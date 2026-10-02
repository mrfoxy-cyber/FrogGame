public class GameStateRepository
{
  private static int _gameStateId = 0;

  public static void Reset()
  {
    _gameStateId = 1;
  }

  public static void SetGameStateId(int id)
  {
    _gameStateId = id;
  }

  public static int GetGameStateId()
  {
    return _gameStateId;
  }
}
