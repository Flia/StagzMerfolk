using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace StagzMerfolk;

    [UsedImplicitly]
    public class StagzMerfolk : Mod
    {
        private readonly StagzMerfolkSettings settings;

        public StagzMerfolk(ModContentPack content) : base(content)
        {
            settings = GetSettings<StagzMerfolkSettings>();
            var harmony = new Harmony("com.arquebus.rimworld.mod.stagzmerfolk");
            harmony.PatchAll();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() => "StagzMerfolk_Settings_Category".Translate();
    }

    public class StagzMerfolkSettings : ModSettings
    {
        public static bool dbhCleaningCountsAsHydration;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref dbhCleaningCountsAsHydration, "StagzMerfolkDbhCleaningCountsAsHydration");
            base.ExposeData();
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            string label = "StagzMerfolk_Settings_DbhCleaningCountsAsHydrationLabel".Translate();
            string tooltip = "StagzMerfolk_Settings_DbhCleaningCountsAsHydrationTooltip".Translate();
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled(label, ref dbhCleaningCountsAsHydration, tooltip);
            listingStandard.End();
        }
    }