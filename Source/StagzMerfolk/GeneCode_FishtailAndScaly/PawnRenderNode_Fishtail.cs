using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace StagzMerfolk;

[UsedImplicitly]
public class PawnRenderNode_Fishtail(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
    : PawnRenderNode(pawn, props, tree)
{
    public override Graphic GraphicFor(Pawn pawn)
    {
        if (pawn.story?.furDef == null)
        {
            return null;
        }

        var skinColor = pawn.story.SkinColor;
        //if (StagzMerfolkSettings.transparentSkinWithoutFur) {skinColor.a = 0;} - for theoretical bodyreplacer compat

        Shader shader = pawn.story.SkinColorOverriden
            ? StagzDefOf.ScalyGenes.Shader 
            : StagzDefOf.ScalyGenesWithSkinShader.Shader;
        
        switch (pawn.Drawer.renderer.CurRotDrawMode)
        {
            case RotDrawMode.Fresh:
                return GraphicDatabase.Get<Graphic_Multi>(
                    pawn.story.furDef.GetFurBodyGraphicPath(pawn),
                    shader,
                    Vector2.one,
                    skinColor,
                    ColorFor(pawn)
                    );
            case RotDrawMode.Rotting:
                return GraphicDatabase.Get<Graphic_Multi>(
                    pawn.story?.furDef.GetFurBodyGraphicPath(pawn),
                    StagzDefOf.ScalyGenes.Shader,
                    Vector2.one,
                    PawnRenderUtility.GetRottenColor(skinColor),
                    ColorFor(pawn)
                    );
            case RotDrawMode.Dessicated:
            default:
                return base.GraphicFor(pawn);
        }
    }

    public override Color ColorFor(Pawn pawn)
    {
        return pawn.GetMerrenScaleColorOrFailsafe();
    }
}