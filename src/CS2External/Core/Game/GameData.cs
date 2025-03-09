using CS2External.Data.Entity;

namespace CS2External.Core.Game;

public class GameData : ThreadedServiceBase
{
    private GameProcess _gameProcess;
    
    public GameData(GameProcess gameProcess)
    {
        _gameProcess = gameProcess;
        Player = new Player();
        Entities = Enumerable.Range(0, 64).Select(index => new Entity(index)).ToArray();
    }

    
    #region Properties

    protected override string ThreadName => nameof(GameData);

    public Player Player { get; private set; }

    public Entity[] Entities { get; private set; }

    #endregion
    
    #region Methods
    
    public override void Dispose()
    {
        base.Dispose();

        Entities = null!;
        Player = null!;
        _gameProcess = null!;
    }

    protected override void FrameAction()
    {
        if (!_gameProcess.IsValid)
        {
            return;
        }
        
        Player.Update(_gameProcess);

        foreach (var entity in Entities)
        {
            entity.Update(_gameProcess);
        }
    }

    #endregion
}