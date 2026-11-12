using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

[HarmonyPatch(typeof(MeditationFocusDef), nameof(MeditationFocusDef.EnablingThingsExplanation))]
public class MeditationFocusDef_EnablingThingsExplanation_Patch
{
    private static bool Prepare() => ModsConfig.RoyaltyActive;

    public static void Postfix(Pawn pawn, MeditationFocusDef __instance, ref string __result)
    {
        if (__instance == StagzDefOf.Stagz_Water && pawn.genes?.HasActiveGene(StagzDefOf.Stagz_Raincaller) == true)
        {
            if (!__result.NullOrEmpty()) __result += "\n";
            __result += $"  - {"StagzMerfolk_UnlockedByGene".Translate()} {StagzDefOf.Stagz_Raincaller.LabelCap}.";
        }
    }
}

[HarmonyPatch(typeof(MeditationFocusTypeAvailabilityCache), "PawnCanUseInt")]
public class MeditationFocusTypeAvailabilityCache_PawnCanUseInt_Patch
{
    private static bool Prepare() => ModsConfig.RoyaltyActive;

    public static void Postfix(Pawn p, MeditationFocusDef type, ref bool __result)
    {
        if (type == StagzDefOf.Stagz_Water && p.genes?.HasActiveGene(StagzDefOf.Stagz_Raincaller) == true)
        {
            __result = true;
        }
    }
}

//frankly this should be a transpiler
[HarmonyPatch(typeof(MeditationUtility), nameof(MeditationUtility.FindMeditationSpot))]
public class FindMeditationSpot_Patch
{
    private static void Postfix(Pawn pawn, ref MeditationSpotAndFocus __result)
    {
        if (!ModLister.CheckRoyalty("Psyfocus") || !StagzDefOf.Stagz_Water.CanPawnUse(pawn)) return;

        float likelihood = RunComparison(pawn, __result.spot);
        
        List<Thing> spotList = pawn.Map.listerThings.ThingsOfDef(StagzDefOf.Stagz_WaterMeditationSpot);
        foreach (LocalTargetInfo item in spotList)
        {
            float newLikelihood = RunComparison(pawn, item);
            if (newLikelihood > likelihood)
            {
                __result.spot = item;
                __result.focus = item;
                likelihood = newLikelihood;
            }
        }
    }

    private static float RunComparison(Pawn pawn, LocalTargetInfo item)
    {
        if (!MeditationUtility.SafeEnvironmentalConditions(pawn, item.Cell, pawn.Map) || !item.Cell.Standable(pawn.Map) || item.Cell.IsForbidden(pawn))
        {
            return float.MinValue;
        }
        float num2 = 1f / MathF.Max(item.Cell.DistanceToSquared(pawn.Position), 0.1f);
        LocalTargetInfo localTargetInfo = (LocalTargetInfo)item.Thing;
        if (pawn.HasPsylink && localTargetInfo.IsValid)
        {
            num2 += localTargetInfo.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn) * 100f;
        }
        Room room = item.Cell.GetRoom(pawn.Map);
        if (room != null && pawn.ownership.OwnedRoom == room)
        {
            num2 += 1f;
        }
        if (item.Thing is Building building && building.GetAssignedPawn() == pawn)
        {
            num2 += building.def == ThingDefOf.MeditationSpot ? 200 : 100;
        }
        if (ModsConfig.IdeologyActive && room?.Role == RoomRoleDefOf.WorshipRoom)
        {
            num2 += 100f;
            foreach (Thing containedAndAdjacentThing in room.ContainedAndAdjacentThings)
            {
                num2 += containedAndAdjacentThing.GetStatValue(StatDefOf.StyleDominance);
            }
        }

        return num2;
    }
}
