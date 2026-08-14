using System;
using HarmonyLib;
using Verse;

namespace StagzMerfolk.DeepSeaCompat;

public static class Helpers
{
    public static bool IsSubmerged(this Pawn pawn)
    {
        if (!ModLister.AnyModActiveNoSuffix(["horizons.deepsea"])) return false;
        
        Func<Map,IntVec3,bool> GetThing = (Func<Map,IntVec3,bool>)AccessTools
                .Method("horizons.deepsea.Core.FloodSim.HDS_SubmergedCellUtility:IsSubmerged")
                .CreateDelegate(typeof(Func<Map,IntVec3,bool>));
        return GetThing(pawn.Map, pawn.Position);
    }
}