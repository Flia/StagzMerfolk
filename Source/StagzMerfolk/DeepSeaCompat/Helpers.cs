using System;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace StagzMerfolk.DeepSeaCompat;

[StaticConstructorOnStartup]
public static class Helpers
{
    private static readonly bool DeepSeaActive;
    private static readonly Func<Pawn, bool> submergedDelegate;
    private static readonly PlanetLayerDef oceanLayer = DefDatabase<PlanetLayerDef>.GetNamed("HDS_Layer_Ocean");
    static Helpers()
    {
        DeepSeaActive = ModLister.AnyModActiveNoSuffix(["horizons.deepsea"]);
        if (DeepSeaActive)
        {
            submergedDelegate = (Func<Pawn, bool>)AccessTools
                .Method("horizons.deepsea.Api.HorizonsDeepseaApi:IsPawnSubmerged")
                .CreateDelegate(typeof(Func<Pawn, bool>));
            if (submergedDelegate is null)
            {
                Log.Error("StagzMerfolk: DeepSea is active, but submergedDelegate failed to fetch");
            }
        }
    }
    public static bool IsSubmerged(this Pawn pawn) => DeepSeaActive && submergedDelegate(pawn);

    public static bool IsSubmerged(this Caravan caravan)
    {
        if (oceanLayer == null) return false;
        var tile = Find.WorldGrid[caravan.Tile] as SurfaceTile;
        return tile?.Layer.Def == oceanLayer;
    }
}