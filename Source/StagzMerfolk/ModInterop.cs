using RimWorld.Planet;
using Verse;
// ReSharper disable MemberCanBePrivate.Global

namespace StagzMerfolk;

//don't forget to nullcheck "this" when adding an extension method
public static class ModInterop
{
    /// <summary>
    /// Hook for underwater mods. Determines whether this pawn should be considered submerged underwater.
    /// Importantly, used by Aquatic Need, Raincaller's and RainVeil's conditional stats, and Fishtail's movement boost.
    /// Less importantly, appears in dryad treasure-finding code and Deep Dive's hediff - those may be reconsidered in the future.
    /// </summary>
    public static bool IsSubmerged(this Pawn pawn) => false;
    
    /// <summary>
    /// Hook for underwater mods. Determines whether this world tile should be considered submerged underwater.
    /// Used by Virtuoso incident pawn spawn code.
    /// Also used by Aquatic Need when the pawn is in caravan, though in the future I may split caravan conditions from the tile it's on (e.g. so merfolk *inside vehicles* wouldn't replenish Aquatic). 
    /// Currently also includes the tile.WaterCovered check: while it is a vanilla check it's never true unless another mod allows caravans over the ocean.
    /// </summary>
    public static bool IsSubmerged(this Tile tile) => tile?.WaterCovered == true;
    
    /// <summary>
    /// Hook for underwater mods. Determines whether this cell on the local map should be considered submerged underwater.
    /// Used by Ariel incident pawn spawn code and Hydration job.
    /// </summary>
    /// <param name="cell">Coordinates of the cell.</param>
    /// <param name="map">Map containing the cell.</param>
    public static bool IsSubmerged(this IntVec3 cell, Map map) => false;
    

    /// <summary>
    /// Hook for mods. Any hediff or job or any other boolean that should restore merfolks' Hydration and isn't covered by its native checks should be patched here.
    /// Keep in mind that pawn.IsSubmerged() is already indirectly checked by the need - no need to repeat it here.
    /// I admit this is a primitive solution for a problem that should be more abstracted, but I lack the knowledge to implement it right now.
    /// </summary>
    public static bool HasModdedConditionRestoringAquaticNeed(this Pawn pawn) =>
        pawn?.health.hediffSet.HasHediff(StagzDefOf.IntheStandaloneHotSpring) == true;
}