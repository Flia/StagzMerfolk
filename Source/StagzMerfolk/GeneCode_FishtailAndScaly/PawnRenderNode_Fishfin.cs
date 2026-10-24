using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace StagzMerfolk;

[UsedImplicitly]
public class PawnRenderNode_Fishfin(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
    : PawnRenderNode(pawn, props, tree)
{
    public override Graphic GraphicFor(Pawn pawn)
    {
        Shader shader = PawnRenderNode_Fishtail.FigureOutTheShader(pawn);
        if (shader == StagzDefOf.ScalyGenesWithSkinShader.Shader || shader == StagzDefOf.ScalyGenes.Shader)
        { 
            return GraphicDatabase.Get<Graphic_Multi>(Props.texPath, DefaultShader, Vector2.one, pawn.GetMerrenScaleColorOrFailsafe());
        }
        return base.GraphicFor(pawn);
    }
}
