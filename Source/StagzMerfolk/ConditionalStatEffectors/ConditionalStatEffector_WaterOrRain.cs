using RimWorld;
using Verse;

namespace StagzMerfolk;

public class ConditionalStatEffector_WaterOrRain : ConditionalStatAffecter
{
    public override bool Applies(StatRequest req)
    {
        //Check if request is valid (request is for a pawn and pawn is on a map
        var pawn = req.Thing as Pawn;
        if (!req.HasThing || pawn?.Map == null) return false;

        //Check if pawn is in water or in rain
        return pawn.OnWater() || pawn.InRain();
    }

    public override string Label => "StagzMerfolk_ConditionalStatEffector_WaterOrRain".Translate();
}