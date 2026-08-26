using System;

namespace StagzMerfolk;

[Obsolete("Left for the sake of ongoing saves. Will be removed with 1.7.")]
public class MentalState_VeryCharmed : MentalState_Charmed
{
    public MentalState_VeryCharmed()
    {
        this.charmChance = 1f;
    }
}