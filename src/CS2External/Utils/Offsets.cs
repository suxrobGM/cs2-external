using System.Dynamic;
using System.Net.Http;
using System.Text.Json;
using CS2Cheat.Core.Data;
using Serilog;

namespace CS2Cheat.Utils;

public abstract class Offsets
{
    public const float WeaponRecoilScale = 2f;
    public static int dwLocalPlayerPawn;
    public static int m_vOldOrigin;
    public static int m_vecViewOffset;
    public static int m_AimPunchAngle;
    public static int m_modelState;
    public static int m_pGameSceneNode;
    public static int m_fFlags;
    public static int m_iIDEntIndex;
    public static int m_lifeState;
    public static int m_iHealth;
    public static int m_iTeamNum;
    public static int dwEntityList;
    public static int m_bDormant;
    public static int m_iShotsFired;
    public static int m_hPawn;
    public static int dwLocalPlayerController;
    public static int dwViewMatrix;
    public static int dwViewAngles;
    public static int m_entitySpottedState;
    public static int m_Item;
    public static int m_pClippingWeapon;
    public static int m_AttributeManager;
    public static int m_iItemDefinitionIndex;
    public static int m_bIsScoped;
    public static int m_flFlashDuration;
    public static int m_iszPlayerName;
    public static int dwPlantedC4;
    public static int dwGlobalVars;
    public static int m_nBombSite;
    public static int m_bBombDefused;
    public static int m_vecAbsVelocity;
    public static int m_flDefuseCountDown;
    public static int m_flC4Blow;
    public static int m_bBeingDefused;
    public const nint m_nCurrentTickThisFrame = 0x34;

    public static readonly Dictionary<string, int> Bones = new()
    {
        { "head", 6 },
        { "neck_0", 5 },
        { "spine_1", 4 },
        { "spine_2", 2 },
        { "pelvis", 0 },
        { "arm_upper_L", 8 },
        { "arm_lower_L", 9 },
        { "hand_L", 10 },
        { "arm_upper_R", 13 },
        { "arm_lower_R", 14 },
        { "hand_R", 15 },
        { "leg_upper_L", 22 },
        { "leg_lower_L", 23 },
        { "ankle_L", 24 },
        { "leg_upper_R", 25 },
        { "leg_lower_R", 26 },
        { "ankle_R", 27 }
    };

    public static async Task UpdateOffsets()
    {
        try
        {
            var offsetsJson = await FetchJson("https://raw.githubusercontent.com/a2x/cs2-dumper/main/output/offsets.json");
            var clientDllJson = await FetchJson("https://raw.githubusercontent.com/a2x/cs2-dumper/main/output/client_dll.json");
            
            var offsets = JsonSerializer.Deserialize<OffsetsDto>(offsetsJson)!;
            var sourceDataClient = JsonSerializer.Deserialize<ClientDllDto>(clientDllJson)!;
            Log.Information("Fetched offsets and client.dll data from GitHub");

            dynamic destData = new ExpandoObject();

            // Offsets
            destData.dwBuildNumber = offsets.Engine2dll.dwBuildNumber;
            destData.dwLocalPlayerController = offsets.ClientDll.dwLocalPlayerController;
            destData.dwEntityList = offsets.ClientDll.dwEntityList;
            destData.dwViewMatrix = offsets.ClientDll.dwViewMatrix;
            destData.dwPlantedC4 = offsets.ClientDll.dwPlantedC4;
            destData.dwLocalPlayerPawn = offsets.ClientDll.dwLocalPlayerPawn;
            destData.dwViewAngles = offsets.ClientDll.dwViewAngles;
            destData.dwPlantedC4 = offsets.ClientDll.dwPlantedC4;
            destData.dwGlobalVars = offsets.ClientDll.dwGlobalVars;

            // client.dll
            destData.m_fFlags = sourceDataClient.ClientDll.classes.C_BaseEntity.fields.m_fFlags;
            destData.m_vOldOrigin = sourceDataClient.ClientDll.classes.C_BasePlayerPawn.fields.m_vOldOrigin;
            destData.m_vecViewOffset =
                sourceDataClient.ClientDll.classes.C_BaseModelEntity.fields.m_vecViewOffset;
            destData.m_aimPunchAngle = sourceDataClient.ClientDll.classes.C_CSPlayerPawn.fields.m_aimPunchAngle;
            destData.m_modelState = sourceDataClient.ClientDll.classes.CSkeletonInstance.fields.m_modelState;
            destData.m_pGameSceneNode = sourceDataClient.ClientDll.classes.C_BaseEntity.fields.m_pGameSceneNode;
            destData.m_iIDEntIndex = sourceDataClient.ClientDll.classes.C_CSPlayerPawnBase.fields.m_iIDEntIndex;
            destData.m_lifeState = sourceDataClient.ClientDll.classes.C_BaseEntity.fields.m_lifeState;
            destData.m_iHealth = sourceDataClient.ClientDll.classes.C_BaseEntity.fields.m_iHealth;
            destData.m_iTeamNum = sourceDataClient.ClientDll.classes.C_BaseEntity.fields.m_iTeamNum;
            destData.m_bDormant = sourceDataClient.ClientDll.classes.CGameSceneNode.fields.m_bDormant;
            destData.m_iShotsFired = sourceDataClient.ClientDll.classes.C_CSPlayerPawn.fields.m_iShotsFired;
            destData.m_hPawn = sourceDataClient.ClientDll.classes.CBasePlayerController.fields.m_hPawn;
            destData.m_entitySpottedState =
                sourceDataClient.ClientDll.classes.C_CSPlayerPawn.fields.m_entitySpottedState;
            destData.m_Item = sourceDataClient.ClientDll.classes.C_AttributeContainer.fields.m_Item;
            destData.m_pClippingWeapon =
                sourceDataClient.ClientDll.classes.C_CSPlayerPawnBase.fields.m_pClippingWeapon;
            destData.m_AttributeManager =
                sourceDataClient.ClientDll.classes.C_EconEntity.fields.m_AttributeManager;
            destData.m_iItemDefinitionIndex =
                sourceDataClient.ClientDll.classes.C_EconItemView.fields.m_iItemDefinitionIndex;
            destData.m_bIsScoped = sourceDataClient.ClientDll.classes.C_CSPlayerPawnBase.fields.m_bIsScoped;
            destData.m_flFlashDuration =
                sourceDataClient.ClientDll.classes.C_CSPlayerPawnBase.fields.m_flFlashDuration;
            destData.m_iszPlayerName =
                sourceDataClient.ClientDll.classes.CBasePlayerController.fields.m_iszPlayerName;
            destData.m_nBombSite = sourceDataClient.ClientDll.classes.C_PlantedC4.fields.m_nBombSite;
            destData.m_bBombDefused = sourceDataClient.ClientDll.classes.C_PlantedC4.fields.m_bBombDefused;
            destData.m_vecAbsVelocity =
                sourceDataClient.ClientDll.classes.C_BaseEntity.fields.m_vecAbsVelocity;
            destData.m_flDefuseCountDown =
                sourceDataClient.ClientDll.classes.C_PlantedC4.fields.m_flDefuseCountDown;
            destData.m_flC4Blow = sourceDataClient.ClientDll.classes.C_PlantedC4.fields.m_flC4Blow;
            destData.m_bBeingDefused = sourceDataClient.ClientDll.classes.C_PlantedC4.fields.m_bBeingDefused;
            
            UpdateStaticFields(destData);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating offsets");
            throw;
        }
    }

    private static async Task<string> FetchJson(string url)
    {
        using var client = new HttpClient();
        return await client.GetStringAsync(url);
    }

    private static void UpdateStaticFields(dynamic data)
    {
        dwLocalPlayerPawn = data.dwLocalPlayerPawn;
        m_vOldOrigin = data.m_vOldOrigin;
        m_vecViewOffset = data.m_vecViewOffset;
        m_AimPunchAngle = data.m_aimPunchAngle;
        m_modelState = data.m_modelState;
        m_pGameSceneNode = data.m_pGameSceneNode;
        m_iIDEntIndex = data.m_iIDEntIndex;
        m_lifeState = data.m_lifeState;
        m_iHealth = data.m_iHealth;
        m_iTeamNum = data.m_iTeamNum;
        m_bDormant = data.m_bDormant;
        m_iShotsFired = data.m_iShotsFired;
        m_hPawn = data.m_hPawn;
        m_fFlags = data.m_fFlags;
        dwLocalPlayerController = data.dwLocalPlayerController;
        dwViewMatrix = data.dwViewMatrix;
        dwViewAngles = data.dwViewAngles;
        dwEntityList = data.dwEntityList;
        m_entitySpottedState = data.m_entitySpottedState;
        m_Item = data.m_Item;
        m_pClippingWeapon = data.m_pClippingWeapon;
        m_AttributeManager = data.m_AttributeManager;
        m_iItemDefinitionIndex = data.m_iItemDefinitionIndex;
        m_bIsScoped = data.m_bIsScoped;
        m_flFlashDuration = data.m_flFlashDuration;
        m_iszPlayerName = data.m_iszPlayerName;
        dwPlantedC4 = data.dwPlantedC4;
        dwGlobalVars = data.dwGlobalVars;
        m_nBombSite = data.m_nBombSite;
        m_bBombDefused = data.m_bBombDefused;
        m_vecAbsVelocity = data.m_vecAbsVelocity;
        m_flDefuseCountDown = data.m_flDefuseCountDown;
        m_flC4Blow = data.m_flC4Blow;
        m_bBeingDefused = data.m_bBeingDefused;
    }
}