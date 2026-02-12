/*
 Was supposed to be used with Watermill.
 Works, but only if you patch Passability to something other than Impassable.
 This issue repeats with basegame Wall/Ascetic. I assume there's a LoS check somewhere but idk how to deal with that.
*/

/*using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StagzMerfolk;

public class FocusStrengthOffset_WaterCandles : FocusStrengthOffset_BuildingDefs
{
    public override float OffsetForBuilding(Thing b)
    {
        return BuildingLit(b) ? OffsetFor(b.def) : 0.0f;
    }

    private bool BuildingLit(Thing b)
    {
        CompGlower comp = b.TryGetComp<CompGlower>();
        return comp != null && comp.Glows;
    }

    public override int BuildingCount(Thing parent)
    {
        string report = "";
        if (parent == null || !parent.Spawned)
            return 0;
        int linkables = 0;
        List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, radius, defs, parent);
        foreach (Thing cell in forCell)
        {
            report = report + cell.def.defName + " ";
            if (BuildingLit(cell))
            {
                report = report + "-accepted ";
                ++linkables; 
            } 
        }
        Log.Message(report + " " + linkables);
        return Math.Min(linkables, maxBuildings);
    }
}*/