using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace StagzMerfolk;

[PublicAPI]
public class GameComp_CharmTracker : GameComponent
{
    public GameComp_CharmTracker(Game game) { }

    public static List<CharmedPawnRecord> pawnsWithPendingConsequences = [];
    public override void GameComponentTick()
    {
        if (Find.TickManager.TicksGame % 3412 != 0) return;
        
        if (pawnsWithPendingConsequences.EnumerableNullOrEmpty()) return;
        for (int index = pawnsWithPendingConsequences.Count - 1; index >= 0; index--)
        {
            CharmedPawnRecord record = pawnsWithPendingConsequences[index];
            if (Find.TickManager.TicksGame < record.triggerConsequenceTick) continue;
            Pawn pawn = record.pawn;
                
            //check for faulty records
            if (pawn == null || pawn.Dead || pawn.everLostEgo || Find.TickManager.TicksGame - record.triggerConsequenceTick > 15 * GenDate.TicksPerDay)
            {
                pawnsWithPendingConsequences.Remove(record);
                continue;
            }
            //delay consequences for pawns which cannot execute them right now
            if (!pawn.Spawned) continue;
            if (pawn.Downed) continue;
            if (pawn.InMentalState) continue;
            if (!record.outcomeDef.canOccurWhenImprisoned && (pawn.IsPrisoner || pawn.IsSlave)) continue;
            record.outcomeDef.Worker.DoOutcome(pawn);
            pawnsWithPendingConsequences.Remove(record);
        }
    }
    
    public override void ExposeData()
    {
        Scribe_Collections.Look(ref pawnsWithPendingConsequences, "pawnsWithPendingConsequences", LookMode.Deep);
    }
}

public class CharmedPawnRecord :  IExposable
{
    public Pawn pawn;
    public int triggerConsequenceTick;
    public CharmOutcomeDef outcomeDef;
    
    //Needed for IExposable to work correctly
    [UsedImplicitly]
    public CharmedPawnRecord() { }
    
    public CharmedPawnRecord(Pawn pawn, int triggerConsequenceTick, CharmOutcomeDef outcomeDef)
    {
        this.pawn = pawn;
        this.triggerConsequenceTick = triggerConsequenceTick;
        this.outcomeDef = outcomeDef;
    }
        
    public void ExposeData()
    {
        Scribe_References.Look(ref pawn, "pawn");
        Scribe_Values.Look(ref triggerConsequenceTick, "triggerConsequenceTick");
        Scribe_Defs.Look(ref outcomeDef, "outcomeDef");
    }
}

