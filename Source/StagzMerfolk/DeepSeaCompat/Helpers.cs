using System;
using HarmonyLib;
using Verse;

namespace StagzMerfolk.DeepSeaCompat;

[StaticConstructorOnStartup]
public static class Helpers
{
    private static readonly bool DeepSeaActive;
    private static readonly Func<Map, IntVec3, bool> submergedDelegate;
    static Helpers()
    {
        DeepSeaActive = ModLister.AnyModActiveNoSuffix(["horizons.deepsea"]);
        if (DeepSeaActive)
        {
            submergedDelegate = (Func<Map, IntVec3, bool>)AccessTools
                .Method("horizons.deepsea.Core.FloodSim.HDS_SubmergedCellUtility:IsSubmerged")
                .CreateDelegate(typeof(Func<Map, IntVec3, bool>));
        }
    }
    public static bool IsSubmerged(this Pawn pawn) => DeepSeaActive && submergedDelegate(pawn.Map, pawn.Position);
}