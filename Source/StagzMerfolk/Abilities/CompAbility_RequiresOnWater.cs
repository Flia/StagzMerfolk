using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace StagzMerfolk;

[PublicAPI]
public class CompAbility_RequiresOnWater : AbilityComp
{
    public override bool GizmoDisabled(out string reason)
    {
        if (!parent.pawn.OnWater())
        {
            reason = "StagzMerfolk_AbilityDisabledNotOnWater".Translate();
            return true;
        }
        reason = null;
        return false;
    }
}