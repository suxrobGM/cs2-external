using System.Runtime.InteropServices;
using CS2Cheat.Data.Game;
using CS2Cheat.Features;
using CS2Cheat.Graphics;
using CS2Cheat.Utils;
using Serilog;
using static CS2Cheat.Core.User32;
using Application = System.Windows.Application;

namespace CS2Cheat;

public class Program : Application, IDisposable
{
    private Program()
    {
        InitializeLogger();
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
        GameProcess = default!;

        GameData.Dispose();
        GameData = default!;

        WindowOverlay.Dispose();
        WindowOverlay = default!;

        Graphics.Dispose();
        Graphics = default!;

        Trigger.Dispose();
        Trigger = default!;

        AimBot.Dispose();
        AimBot = default!;

        BombTimer.Dispose();
        BombTimer = default!;
    }

    public static void Main()
    {
        new Program().Run();
    }

    private void InitializeComponent()
    {
        var features = ConfigManager.Load();
        Log.Information("Starting CS2Cheat with features: {Features}", features);
        
        GameProcess = new GameProcess();
        GameProcess.Start();

        GameData = new GameData(GameProcess);
        GameData.Start();

        WindowOverlay = new WindowOverlay(GameProcess);
        WindowOverlay.Start();

        Graphics = new Graphics.Graphics(GameProcess, GameData, WindowOverlay);
        Graphics.Start();

        Trigger = new TriggerBot(GameProcess, GameData);
        if (features.TriggerBot) Trigger.Start();

        AimBot = new AimBot(GameProcess, GameData);
        if (features.AimBot) AimBot.Start();

        BombTimer = new BombTimer(Graphics);
        if (features.BombTimer) BombTimer.Start();

        var result = SetWindowDisplayAffinity(WindowOverlay.Window.Handle, 0x00000011); //obs bypass
        
        if (result == 0)
        {
            Log.Error("Failed to set window display affinity. Error code: {ErrorCode}", Marshal.GetLastWin32Error());
        }
        else
        {
            Log.Information("Window display affinity set successfully");
        }
    }

    private static void InitializeLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }
}
