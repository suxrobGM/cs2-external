using CS2External.Data.Entity;

namespace CS2External.Core.Game;

public class GameData : ThreadedServiceBase
{
    #region properties

    protected override string ThreadName => nameof(GameData);

    private GameProcess GameProcess { get; set; }

    public Player Player { get; private set; }

    public Entity[] Entities { get; private set; }

    #endregion

    #region methods

    /// <inheritdoc />
    public GameData(GameProcess gameProcess)
    {
        GameProcess = gameProcess;
        Player = new Player();
        Entities = Enumerable.Range(0, 64).Select(index => new Entity(index)).ToArray();
    }

    public override void Dispose()
    {
        base.Dispose();

        Entities = null;
        Player = null;
        GameProcess = null;
    }

    protected override void FrameAction()
    {
        if (!GameProcess.IsValid) return;
        Player.Update(GameProcess);

        foreach (var entity in Entities) entity.Update(GameProcess);
    }

    #endregion
}