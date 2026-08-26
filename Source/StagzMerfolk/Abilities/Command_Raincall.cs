using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

namespace StagzMerfolk;

[UsedImplicitly]
public class Command_Rainfall : Command_Ability
{
    private readonly CompAbilityEffect_CallWeather cachedComp;
    public Command_Rainfall(Ability ability, Pawn pawn) : base(ability, pawn)
    {
        try
        {
            cachedComp = Ability.comps.OfType<CompAbilityEffect_CallWeather>().First();
        } catch
        {
            Log.Error("CompAbilityEffect_CallWeather is missing from Raincall.");
        }
    }

    public override void ProcessInput(Event ev)
    {
        cachedComp.weatherToBeCalled = cachedComp.Props.weatherDefsForRandomRoll.RandomElement();
        base.ProcessInput(ev);
    }

    public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
    {
        get
        {
            foreach (WeatherDef weather in cachedComp.Props.weatherDefsForSelection)
            {
                yield return new FloatMenuOption(weather.label, delegate
                {
                    cachedComp.weatherToBeCalled = weather;
                    //will only ever work as untargetable ability unless rewritten
                    ability.QueueCastingJob((LocalTargetInfo) (Thing) ability.pawn, LocalTargetInfo.Invalid);
                });
            }
        }
    }
}