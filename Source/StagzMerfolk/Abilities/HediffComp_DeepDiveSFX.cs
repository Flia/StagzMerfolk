using JetBrains.Annotations;
using Verse;
using Verse.Sound;

namespace StagzMerfolk;

public class HediffComp_DeepDiveSFX : HediffComp
{
    public HediffCompProperties_DeepDiveSFX Props => (HediffCompProperties_DeepDiveSFX) props;
    public override void CompPostPostAdd(DamageInfo? dinfo)
    {
        Props.effecterDefForAdding?.SpawnAttached(Pawn, Pawn.Map);
        Props.soundDefForAdding?.PlayOneShot(Pawn);
    }

    public override void CompPostPostRemoved()
    {
        Props.effecterDefForRemoving?.SpawnAttached(Pawn, Pawn.Map);
        Props.soundDefForRemoving?.PlayOneShot(Pawn);
    }
}
[PublicAPI]
public class HediffCompProperties_DeepDiveSFX : HediffCompProperties
{
    public EffecterDef effecterDefForAdding;
    public SoundDef soundDefForAdding;
    public EffecterDef effecterDefForRemoving;
    public SoundDef soundDefForRemoving;
    public HediffCompProperties_DeepDiveSFX()
    {
        compClass = typeof(HediffComp_DeepDiveSFX);
    }
}