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
        
        Color skinColor = ColorFor(pawn);
        //if (StagzMerfolkSettings.transparentSkinWithoutFur) {skinColor.a = 0;} - for theoretical bodyreplacer compat

        return GraphicDatabase.Get<Graphic_Multi>(
            pawn.story.furDef.GetFurBodyGraphicPath(pawn), 
            ThisShouldveBeenJustOverridden_ShaderFor(pawn), 
            Vector2.one, 
            skinColor, 
            pawn.Drawer.renderer.StatueColor ?? pawn.GetMerrenScaleColorOrFailsafe());
    }

    private Shader ThisShouldveBeenJustOverridden_ShaderFor(Pawn pawn)
    {
        Shader shader = ShaderFor(pawn);
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