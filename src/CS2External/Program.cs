using System.IO;
using CS2External.Core.Game;
using CS2External.Core.Options;
using CS2External.Features;
using CS2External.Graphics;
using CS2External.Utils;
using Microsoft.Extensions.Configuration;
using Serilog;
using static CS2External.Core.User32;
using Application = System.Windows.Application;

namespace CS2External;

public class Program : Application, IDisposable
{
    private GameProcess _gameProcess = null!;
    private GameData _gameData = null!;
    private WindowOverlay _windowOverlay = null!;
    private Graphics.Graphics _graphics = null!;
    private TriggerBot _triggerBot = null!;
    private AimBot _aimBot = null!;
    private BombTimer _bombTimer = null!;
    
    public static IConfiguration Configuration { get; } = InitConfiguration();
    
    private Program()
    {
        InitLogger();
        _ = Offsets.UpdateOffsets();
        Startup += (_, _) => InitializeComponent();
        Exit += (_, _) => Dispose();
    }
    
    public static void Main()
    {
        new Program().Run();
    }

    public void Dispose()
    {
        _gameProcess.Dispose();
        _gameProcess = null!;

        _gameData.Dispose();
        _gameData = null!;

        _windowOverlay.Dispose();
        _windowOverlay = null!;

        _graphics.Dispose();
        _graphics = null!;

        _triggerBot.Dispose();
        _triggerBot = null!;

        _aimBot.Dispose();
        _aimBot = null!;

        _bombTimer.Dispose();
        _bombTimer = null!;
        GC.SuppressFinalize(this);
    }

    private void InitializeComponent()
    {
        var features = Configuration.GetSection("Features").Get<FeaturesOptions>();

        if (features is null)
        {
            Log.Warning("Failed to load features options from the appsettings.json file. Initializing with default settings.");
            features = FeaturesOptions.Default();
        }
        
        _gameProcess = new GameProcess();
        _gameProcess.Start();

        _gameData = new GameData(_gameProcess);
        _gameData.Start();

        _windowOverlay = new WindowOverlay(_gameProcess);
        _windowOverlay.Start();

        _graphics = new Graphics.Graphics(_gameProcess, _gameData, _windowOverlay);
        _graphics.Start();

        _triggerBot = new TriggerBot(_gameProcess, _gameData);
        if (features.TriggerBot)
        {
            _triggerBot.Start();
        }

        _aimBot = new AimBot(_gameProcess, _gameData);
        if (features.AimBot)
        {
            _aimBot.Start();
        }

        _bombTimer = new BombTimer(_graphics);
        if (features.BombTimer)
        {
            _bombTimer.Start();
        }

        SetWindowDisplayAffinity(_windowOverlay.Window.Handle, 0x00000011); //obs bypass
    }

    private static IConfiguration InitConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    private static void InitLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
    }
}