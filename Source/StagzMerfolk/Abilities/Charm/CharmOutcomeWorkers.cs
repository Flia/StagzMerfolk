using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace StagzMerfolk;

[PublicAPI]
public class CharmOutcomeWorker
{
    public CharmOutcomeDef def;
    public virtual void DoOutcome(Pawn pawn) { }
}

[PublicAPI]
public class CharmOutcomeWorker_Depart : CharmOutcomeWorker
{
    public override void DoOutcome(Pawn pawn)
    {
        pawn.mindState.mentalStateHandler
            .TryStartMentalState(StagzDefOf.GiveUpExit, 
                "StagzMerfolk_WasPreviouslyCharmed".Translate().Formatted(pawn.Named("PAWN")), forceWake: true);
    }
}

[PublicAPI]
public class CharmOutcomeWorker_Aggressive : CharmOutcomeWorker
{
    public override void DoOutcome(Pawn pawn)
    {
        pawn.mindState.mentalStateHandler
            .TryStartMentalState(MentalStateDefOf.Berserk, 
                "StagzMerfolk_WasPreviouslyCharmed".Translate().Formatted(pawn.Named("PAWN")), forceWake: true);

    }
}