using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace StagzMerfolk;

[UsedImplicitly]
public class PawnRenderNode_ColoredFinEars(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
    : PawnRenderNode_AttachmentHead(pawn, props, tree)
{
    public override Graphic GraphicFor(Pawn pawn)
    {
        Shader shader = PawnRenderNode_Fishtail.FigureOutTheShader(pawn);

        return pawn.Drawer.renderer.CurRotDrawMode is RotDrawMode.Fresh or RotDrawMode.Rotting
            ? GraphicDatabase.Get<Graphic_Multi>(Props.texPath, shader, Vector2.one, ColorFor(pawn), pawn.GetMerrenScaleColorOrFailsafe())
            : base.GraphicFor(pawn);
    }
}