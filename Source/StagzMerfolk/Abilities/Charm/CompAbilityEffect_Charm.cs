using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace StagzMerfolk;

[UsedImplicitly]
public class CompAbilityEffect_Charm : CompAbilityEffect
{
    public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
    {
        return Valid(target);
    }

    public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
    {
        if (target.Thing is not Pawn targetPawn) return false;
        return base.Valid(target, throwMessages) &&
               ValidateDifferentFaction(targetPawn, throwMessages, parent) &&
               AbilityUtility.ValidateNoMentalState(targetPawn, throwMessages, parent);
    }

    private static bool ValidateDifferentFaction(Pawn targetPawn, bool throwMessages, Ability ability)
    {
        if (targetPawn.Faction != ability.pawn.Faction) return true;
        if (throwMessages)
        {
            //Can't interpolate the whole string at once - breaks in game
            Messages.Message(
                $"{"CannotUseAbility".Translate(ability.def.label)}: " + "StagzMerfolk_AbilityMustBeDifferentFaction".Translate(targetPawn, ability.pawn),
                targetPawn,
                MessageTypeDefOf.RejectInput,
                false);
        }
        return false;
    }
    
    public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
    {
        if (target.Thing is not Pawn targetPawn) return;
        base.Apply(target, dest);
        
        RecruitUtility.Recruit(targetPawn, Faction.OfPlayer);

        if (targetPawn.IsAnimal)
        {
            TryDevelopBondRelation_Modified(parent.pawn, targetPawn);
        } else if (targetPawn.RaceProps.Humanlike)
        {
            RollCharmOutcomesForHumanlikes(targetPawn);
        }

        targetPawn.health.AddHediff(StagzDefOf.Stagz_Infatuated);
        
        if (targetPawn.IsAnimal)
        {
            Find.LetterStack.ReceiveLetter(
                "StagzMerfolk_LetterLabel_CharmedAnimal".Translate().Formatted(targetPawn.Named("PAWN")),
                "StagzMerfolk_LetterText_CharmedAnimal".Translate().Formatted(targetPawn.Named("PAWN")),
                LetterDefOf.PositiveEvent,
                new LookTargets(targetPawn));
        } else if (targetPawn.RaceProps.Humanlike)
        {
            Find.LetterStack.ReceiveLetter(
                "StagzMerfolk_LetterLabel_CharmedHumanlike".Translate().Formatted(targetPawn.Named("PAWN")),
                "StagzMerfolk_LetterText_CharmedHumanlike".Translate().Formatted(targetPawn.Named("PAWN")),
                LetterDefOf.PositiveEvent,
                new LookTargets(targetPawn));
        }
    }

    private static void RollCharmOutcomesForHumanlikes(Pawn targetPawn)
    {
        var outcome = CharmOutcome.Defs.RandomElementByWeight(rawr => rawr.weight);
        var delayTicksRoll = (int) (Rand.Range(outcome.triggersAfterDays.min, outcome.triggersAfterDays.max) * GenDate.TicksPerDay);
        GameComp_CharmTracker.pawnsWithPendingConsequences.Add(
            new CharmedPawnRecord(targetPawn, Find.TickManager.TicksGame + delayTicksRoll, outcome));
    }
    
    
    //RelationsUtility.TryDevelopBondRelation. The alternative was trying to make a transpiler which I wasn't sure how to make
    //Changes: dropped all chance checks, works with any ideo, works on any trainability even pen animals
    private static bool TryDevelopBondRelation_Modified(Pawn humanlike, Pawn animal)
    {
        if (!animal.IsAnimal || animal.Faction == Faction.OfPlayer && humanlike.IsQuestLodger())
        {
            return false;
        }
        
        //to be honest first two should never happen - charm only works on non-colonist pawns in the first place
        if (humanlike.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal) || 
            animal.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, x => x.Spawned) != null || 
            humanlike.story.traits.HasTrait(TraitDefOf.Psychopath) || 
            humanlike.Inhumanized())
        {
            return false;
        }
        
        humanlike.relations.AddDirectRelation(PawnRelationDefOf.Bond, animal);
        if (humanlike.Faction == Faction.OfPlayer || animal.Faction == Faction.OfPlayer)
        {
            TaleRecorder.RecordTale(TaleDefOf.BondedWithAnimal, humanlike, animal);
        }
        
        bool flag = false;
        string oldName = null;
        if (animal.Name == null || animal.Name.Numerical)
        {
            flag = true;
            oldName = animal.Name == null ? animal.LabelIndefinite() : animal.Name.ToStringFull;
            animal.Name = PawnBioAndNameGenerator.GeneratePawnName(animal);
        }
        if (PawnUtility.ShouldSendNotificationAbout(humanlike) || PawnUtility.ShouldSendNotificationAbout(animal))
        {
            string text = flag
                ? "MessageNewBondRelationNewName"
                    .Translate(humanlike.LabelShort, oldName, animal.Name.ToStringFull, humanlike.Named("HUMAN"), animal.Named("ANIMAL"))
                    .AdjustedFor(animal)
                    .CapitalizeFirst()
                : "MessageNewBondRelation"
                    .Translate(humanlike.LabelShort, animal.LabelShort, humanlike.Named("HUMAN"), animal.Named("ANIMAL"))
                    .CapitalizeFirst();
            Messages.Message(text, humanlike, MessageTypeDefOf.PositiveEvent);
        }
        return true;
    }
}