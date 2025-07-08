using System.IO;
using System.Text.Json;
using Serilog;
using Keys = Process.NET.Native.Types.Keys;

namespace CS2Cheat.Utils;

public class ConfigManager
{
    private const string ConfigFile = "config.json";
    private static JsonSerializerOptions _serializeOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };
    
    public bool AimBot { get; set; }
    public bool BombTimer { get; set; }
    public bool EspAimCrosshair { get; set; }
    public bool EspBox { get; set; }
    public bool SkeletonEsp { get; set; }
    public bool TriggerBot { get; set; }
    public Keys AimBotKey { get; set; }
    public Keys TriggerBotKey { get; set; }
    public bool TeamCheck { get; set; }

    public static ConfigManager Load()
    {
        try
        {
            if (!File.Exists(ConfigFile))
            {
                var defaultOptions = Default();
                Save(defaultOptions);
                return defaultOptions;
            }

            var json = File.ReadAllText(ConfigFile);
            var options = JsonSerializer.Deserialize<ConfigManager>(json, _serializeOptions);
            return options ?? Default();
        }
        catch (JsonException)
        {
            return Default();
        }
    }

    public static void Save(ConfigManager options)
    {
        try
        {
            var json = JsonSerializer.Serialize(options, _serializeOptions);
            File.WriteAllText(ConfigFile, json);
            Log.Information("Config options saved successfully to {ConfigFile}.", ConfigFile);
        }
        catch (JsonException)
        {
            Log.Error("Failed to serialize config options.");
        }
    }

    public static ConfigManager Default()
    {
        return new ConfigManager
        {
            AimBot = false,
            BombTimer = true,
            EspAimCrosshair = true,
            EspBox = true,
            SkeletonEsp = true,
            TriggerBot = true,
            AimBotKey = Keys.LButton, // https://github.com/lolp1/Process.NET/blob/ce9ac9cceb2afb30c9288495615c6f3aa34bc1f8/src/Process.NET/Native/Types/NativeEnums.cs#L235
            TriggerBotKey = Keys.LMenu,
            TeamCheck = true
        };
    }

    public override string ToString()
    {
        return $"AimBot: {AimBot}, BombTimer: {BombTimer}, EspAimCrosshair: {EspAimCrosshair}, " +
               $"EspBox: {EspBox}, SkeletonEsp: {SkeletonEsp}, TriggerBot: {TriggerBot}, " +
               $"AimBotKey: {AimBotKey}, TriggerBotKey: {TriggerBotKey}, TeamCheck: {TeamCheck}";
    }
}
