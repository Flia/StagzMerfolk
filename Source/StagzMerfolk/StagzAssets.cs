using UnityEngine;
using Verse;

namespace StagzMerfolk;

[StaticConstructorOnStartup]
public static class StagzAssets
{
    public static readonly Texture2D DyeDialogRotatePawn = ContentFinder<Texture2D>.Get("UI/Buttons/DyeDialogRotatePawn");
}