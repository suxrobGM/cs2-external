namespace CS2External.Core.Options;

public record FeaturesOptions
{
    public bool AimBot { get; set; }
    public bool BombTimer { get; set; }
    public bool EspAimCrosshair { get; set; }
    public bool EspBox { get; set; }
    public bool SkeletonEsp { get; set; }
    public bool TriggerBot { get; set; }

    public static FeaturesOptions Default()
    {
        return new FeaturesOptions
        {
            AimBot = true,
            BombTimer = true,
            EspAimCrosshair = true,
            EspBox = true,
            SkeletonEsp = true,
            TriggerBot = true
        };
    }
}