using System;
using HarmonyLib;
using Verse;

namespace StagzMerfolk.DeepSeaCompat;

[StaticConstructorOnStartup]
public static class Helpers
{
    private static readonly bool DeepSeaActive;
    private static readonly Func<Pawn, bool> submergedDelegate;
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
}