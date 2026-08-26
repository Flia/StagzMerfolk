using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace StagzMerfolk;

//Follows CreepJoinerDownsideDef logic, but stripped of most stuff
//Some stuff like letter fields could be easily added back in - maybe for later expansion
[PublicAPI]
public class CharmOutcomeDef : Def
{
    public float weight = 1f;
    public FloatRange triggersAfterDays = FloatRange.Zero;
    public Type workerClass = typeof(CharmOutcomeWorker);
    public bool canOccurWhenImprisoned;
    
    [Unsaved]
    private CharmOutcomeWorker workerInt;
    public CharmOutcomeWorker Worker
    {
        get
        {
            if (workerInt == null)
            {
                workerInt = (CharmOutcomeWorker)Activator.CreateInstance(workerClass);
                workerInt.def = this;
            }
            return workerInt;
        }
    }
}

[StaticConstructorOnStartup]
public static class CharmOutcome
{
    public static readonly List<CharmOutcomeDef> Defs = DefDatabase<CharmOutcomeDef>.AllDefsListForReading;
}