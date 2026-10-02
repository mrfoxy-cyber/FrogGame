public static class GameServerConstants
{
  public static class GameInformationManagerRpcDetails
  {
    public static ushort ComponentId = 0xF000;
    public static ushort MethodId = 0;
  }

  public static class CharacterSpawnerRpcDetails
  {
    public static ushort ComponentId = 0xF001;
    public static ushort SpawnMethodId = 0;
    public static ushort RespawnMethodId = 1;
  }

  public static class WeaponSpawnerRpcDetails
  {
    public static ushort ComponentId = 0xF002;
    public static ushort SpawnWeaponMethodId = 0;
  }

  public static class GameNotificaitonManagerRpcDetails
  {
    public static ushort ComponentId = 0xF004;
  }

  public static class GameRoundInformationManagerRpcDetails
  {
    public static ushort ComponentId = 0xF005;
  }
}