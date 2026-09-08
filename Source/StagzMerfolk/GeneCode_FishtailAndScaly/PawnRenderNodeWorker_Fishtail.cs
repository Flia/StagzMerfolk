using RimWorld;
using UnityEngine;
using Verse;

namespace StagzMerfolk;

public class PawnRenderNodeWorker_Fishtail : PawnRenderNodeWorker_Fur
{
    //repeats what PawnRenderNodeWorker_AttachmentBody does tbh
    public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
    {
        if (parms.pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated &&
            (parms.pawn.story.bodyType == BodyTypeDefOf.Child || parms.pawn.story.bodyType == BodyTypeDefOf.Baby))
        {
            Vector2 bodyGraphicScale = parms.pawn.story.bodyType.bodyGraphicScale;
            double num = (bodyGraphicScale.x + bodyGraphicScale.y) / 2.0;
            return base.ScaleFor(node, parms) * (float) num;
        }

        return base.ScaleFor(node, parms);
    }
}