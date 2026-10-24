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
        Shader shader = FigureOutTheShader(pawn);
        
        Color skinColor = ColorFor(pawn);
        //if (StagzMerfolkSettings.transparentSkinWithoutFur) {skinColor.a = 0;} - for theoretical bodyreplacer compat

        return pawn.Drawer.renderer.CurRotDrawMode is RotDrawMode.Fresh or RotDrawMode.Rotting 
            ? GraphicDatabase.Get<Graphic_Multi>(pawn.story.furDef.GetFurBodyGraphicPath(pawn), shader, Vector2.one, skinColor, pawn.GetMerrenScaleColorOrFailsafe()) 
            : base.GraphicFor(pawn);
    }

    public static Shader FigureOutTheShader(Pawn pawn)
    {
        Shader shader = ShaderUtility.GetSkinShader(pawn);
        if (shader == ShaderDatabase.CutoutSkin)
        {
            return StagzDefOf.ScalyGenesWithSkinShader.Shader;
        }
        if (shader == ShaderDatabase.CutoutSkinColorOverride || shader == ShaderDatabase.Cutout)
        {
            return StagzDefOf.ScalyGenes.Shader;
        }
        //metalblood uses custom skin shader, possibly other mods
        return shader;
    }
}