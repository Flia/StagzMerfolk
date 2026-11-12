using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace StagzMerfolk;

//todo
[UsedImplicitly]
public class FocusStrengthOffset_Underwater : FocusStrengthOffset
{
    public override string GetExplanation(Thing parent) =>
        CanApply(parent)
            ? "StagzMerfolk_StatsReport_UnderwaterFocusOffset".Translate(GetOffset(parent).ToStringWithSign("0%"))
            : GetExplanationAbstract();

    public override string GetExplanationAbstract(ThingDef def = null) => 
        "StagzMerfolk_StatsReport_UnderwaterFocusOffsetAbstract".Translate(offset.ToStringWithSign("0%"));

    public override float GetOffset(Thing parent, Pawn user = null) => offset;

    public override bool CanApply(Thing parent, Pawn user = null) => 
        base.CanApply(parent, user) && parent.Position.IsSubmerged(parent.Map);
}