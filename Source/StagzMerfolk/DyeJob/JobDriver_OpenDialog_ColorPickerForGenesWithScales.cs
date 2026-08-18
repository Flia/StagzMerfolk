using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;
using Verse.AI;

namespace StagzMerfolk;

[PublicAPI]
public class JobDriver_OpenDialog_ColorPickerForGenesWithScales : JobDriver
{
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(job.targetA, job, errorOnFailed: errorOnFailed);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell)
            .FailOnDespawnedOrNull(TargetIndex.A);
        yield return Toils_General.Do(delegate
        {
            Find.WindowStack.Add(new Dialog_ColorPickerForGenesWithScales(pawn, job.targetA.Thing));
        });
    }
}