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
        return GraphicDatabase.Get<Graphic_Multi>(
            Props.texPath, 
            ThisShouldveBeenJustOverridden_ShaderFor(pawn), 
            Vector2.one, 
            pawn.Drawer.renderer.StatueColor ?? pawn.GetMerrenScaleColorOrFailsafe());
    }
    
    private Shader ThisShouldveBeenJustOverridden_ShaderFor(Pawn pawn)
    {
        Shader shader = ShaderFor(pawn);
        if (shader == ShaderDatabase.CutoutSkin || shader == ShaderDatabase.CutoutSkinColorOverride)
        {
            return ShaderDatabase.Cutout;
        }
        //metalblood uses custom skin shader, possibly other mods
        return shader;
    }
}
