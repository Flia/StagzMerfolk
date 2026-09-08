using UnityEngine;
using Verse;

namespace StagzMerfolk;

public class PawnRenderNodeWorker_Fishfin : PawnRenderNodeWorker_AttachmentBody
{
    public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
    {
        return base.OffsetFor(node, parms, out pivot) * parms.pawn.BodySize;
    }
}