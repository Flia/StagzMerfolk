using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

//These classes only exist because vanilla Filth cannot have programmatically set Color
namespace StagzMerfolk;

[UsedImplicitly]
public class FilthWithColor : Filth
{
    private Color drawColor = Color.white;

    public override Color DrawColor
    {
        get => drawColor;
        set => drawColor = value;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref drawColor, "drawColor");
    }
}


[UsedImplicitly]
public class Graphic_DyeCluster : Graphic_Cluster
{
    public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo) 
        => GraphicDatabase.Get<Graphic_Cluster>(path, newShader, drawSize, newColor, Color.white, data);
}


[UsedImplicitly]
public class Graphic_DyeClusterTight : Graphic_ClusterTight
{
    public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo) 
        => GraphicDatabase.Get<Graphic_ClusterTight>(path, newShader, drawSize, newColor, Color.white, data);
}