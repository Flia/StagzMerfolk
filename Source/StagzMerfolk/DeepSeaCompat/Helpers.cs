using System;
using System.Reflection;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace StagzMerfolk.DeepSeaCompat;

[StaticConstructorOnStartup]
public static class Helpers
{
    private static readonly Func<Pawn, bool> submergedDelegate;
    private static readonly Func<PlanetTile, bool> oceanTileDelegate;

    static Helpers()
    {
        if (!ModLister.AnyModActiveNoSuffix(["horizons.deepsea"])) return;
        submergedDelegate = Bind<Func<Pawn, bool>>("IsPawnSubmerged");
        oceanTileDelegate = Bind<Func<PlanetTile, bool>>("IsUnderwaterTile");
    }

    private static T Bind<T>(string name) where T : class
    {
        MethodInfo method = AccessTools.Method($"horizons.deepsea.Api.HorizonsDeepseaApi:{name}");
        if (method != null) return Delegate.CreateDelegate(typeof(T), method) as T;

        Log.Error($"StagzMerfolk: DeepSea is active, but {name} could not be bound");
        return null;
    }

    public static bool IsSubmerged(this Pawn pawn) =>
        submergedDelegate != null && submergedDelegate(pawn);

    public static bool IsSubmerged(this Caravan caravan) =>
        oceanTileDelegate != null && caravan != null && oceanTileDelegate(caravan.Tile);
}