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
        return GraphicDatabase.Get<Graphic_Multi>(
            Props.texPath, 
            ThisShouldveBeenJustOverridden_ShaderFor(pawn), 
            Vector2.one, 
            ColorFor(pawn), 
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