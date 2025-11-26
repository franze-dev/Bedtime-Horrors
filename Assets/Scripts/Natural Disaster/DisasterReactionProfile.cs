using UnityEngine;

[CreateAssetMenu(fileName = "ReactionProfile", menuName = "ScriptableObjects/NaturalDisasters/ReactionProfile")]
public class DisasterReactionProfile : ScriptableObject
{
    [System.Serializable]
    public class ReactionEntry
    {
        public NaturalDisaster Disaster;
        public DisasterPhase Phase;
        public string AnimationName;
        public int PlayTimes = 1;
        public bool DisableOnEnd = false;
        public bool ClearAnimationOnEnd = false;
    }

    [System.Serializable]
    public class IdleEntry
    {
        public string AnimationName;
        public int PlayTimes = 0;
    }

    public ReactionEntry[] Reactions;
    public IdleEntry Idle;
}


public enum DisasterPhase
{
    None,
    Start,
    Loop,
    End
}