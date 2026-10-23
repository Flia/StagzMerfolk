using System;
using System.Reflection;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace StagzMerfolk;

[StaticConstructorOnStartup]
public static class DeepSea
{
    public static readonly Func<Pawn, bool> pawnSubmergedDelegate;
    public static readonly Func<PlanetTile, bool> worldtileSubmergedDelegate;
    public static readonly Func<Map, IntVec3, float> cellSubmergedDelegate;

    static DeepSea()
    {
        if (!ModLister.AnyModActiveNoSuffix(["horizons.deepsea"])) return;
        pawnSubmergedDelegate = Bind<Func<Pawn, bool>>("IsPawnSubmerged");
        worldtileSubmergedDelegate = Bind<Func<PlanetTile, bool>>("IsUnderwaterTile");
        cellSubmergedDelegate = Bind<Func<Map, IntVec3, float>>("WaterLevelAtCell");
    }

    private static T Bind<T>(string name) where T : class
    {
        MethodInfo method = AccessTools.Method($"horizons.deepsea.Api.HorizonsDeepseaApi:{name}");
        if (method != null) return Delegate.CreateDelegate(typeof(T), method) as T;

        Log.Error($"StagzMerfolk: DeepSea is active, but {name} could not be bound");
        return null;
    }
}