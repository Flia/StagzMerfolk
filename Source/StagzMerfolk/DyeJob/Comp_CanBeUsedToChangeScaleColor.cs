using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace StagzMerfolk;

public class Comp_CanBeUsedToChangeScaleColor: ThingComp
{
    public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
    {
        if (selPawn.TryGetMerrenScaleColor() == null)
        {
            yield break;
        }
        if (!selPawn.CanReach(parent, PathEndMode.OnCell, Danger.Deadly))
        {
            yield return new FloatMenuOption("CannotUseReason".Translate("NoPath".Translate().CapitalizeFirst()), null, MenuOptionPriority.Low);
            yield break;
        }
        
        Thing dye = GenClosest.ClosestThing_Global_Reachable(selPawn.Position, selPawn.Map, selPawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye), PathEndMode.ClosestTouch, TraverseParms.For(selPawn), validator: x => !x.IsForbidden(selPawn) && selPawn.CanReserve(x, 1, 1));
        if (dye == null)
        {
            yield return new FloatMenuOption("CannotUseReason".Translate("StagzMerfolk_FloatMenu_NoReachableDye".Translate().CapitalizeFirst()), null, MenuOptionPriority.Low);
            yield break;
        }
        yield return FloatMenuUtility.DecoratePrioritizedTask(
            new FloatMenuOption("StagzMerfolk_FloatMenu_ChangeMerrenScaleColor".Translate().CapitalizeFirst(),
                delegate
                {
                    selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(StagzDefOf.Stagz_OpenDialog_ColorPickerForGenesWithScales, parent), JobTag.Misc);
                }, MenuOptionPriority.Low), selPawn, parent);
    }
}