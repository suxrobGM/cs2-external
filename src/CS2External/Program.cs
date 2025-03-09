using CS2External.Core.Game;
using CS2External.Features;
using CS2External.Graphics;
using CS2External.Utils;
using Serilog;
using static CS2External.Core.User32;
using Application = System.Windows.Application;

namespace CS2External;

public class Program : Application, IDisposable
{
    private Program()
    {
        InitLogger();
        _ = Offsets.UpdateOffsets();
        Startup += (_, _) => InitializeComponent();
        Exit += (_, _) => Dispose();
    }

    private GameProcess GameProcess { get; set; } = null!;

    private GameData GameData { get; set; } = null!;

    private WindowOverlay WindowOverlay { get; set; } = null!;

    private Graphics.Graphics Graphics { get; set; } = null!;

    private TriggerBot Trigger { get; set; } = null!;

    private AimBot AimBot { get; set; } = null!;

    private BombTimer BombTimer { get; set; } = null!;

    public void Dispose()
    {
        GameProcess.Dispose();
        GameProcess = null!;

        GameData.Dispose();
        GameData = null!;

        WindowOverlay.Dispose();
        WindowOverlay = null!;

        Graphics.Dispose();
        Graphics = null!;

        Trigger.Dispose();
        Trigger = null!;

        AimBot.Dispose();
        AimBot = null!;

        BombTimer.Dispose();
        BombTimer = null!;
        GC.SuppressFinalize(this);
    }

    public static void Main()
    {
        new Program().Run();
    }

    private void InitializeComponent()
    {
        GameProcess = new GameProcess();
        GameProcess.Start();

        GameData = new GameData(GameProcess);
        GameData.Start();

        WindowOverlay = new WindowOverlay(GameProcess);
        WindowOverlay.Start();

        Graphics = new Graphics.Graphics(GameProcess, GameData, WindowOverlay);
        Graphics.Start();

        Trigger = new TriggerBot(GameProcess, GameData);
        Trigger.Start();

        AimBot = new AimBot(GameProcess, GameData);
        AimBot.Start();

        BombTimer = new BombTimer(Graphics);
        BombTimer.Start();

        SetWindowDisplayAffinity(WindowOverlay.Window.Handle, 0x00000011); //obs bypass
    }

    private static void InitLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
    }
}