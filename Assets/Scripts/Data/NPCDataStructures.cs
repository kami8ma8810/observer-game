using System;

namespace JusticeManGO.Data
{
    [Serializable]
    public class NPCDataContainer
    {
        public NPCData[] npcs;
        public GlobalSettings global_settings;
    }

    [Serializable]
    public class NPCData
    {
        public string id;
        public string displayName;
        public string violationType;
        public string description;
        public NPCReactions reactions;
    }

    [Serializable]
    public class NPCReactions
    {
        public NPCReaction report_success;
        public NPCReaction report_failure;
        public NPCReaction excessive_justice;
        public NPCReaction sympathy;
        public NPCReaction reverse_backlash;
        public NPCReaction indifference;
        public NPCReaction light_backlash;
    }

    [Serializable]
    public class NPCReaction
    {
        public float probability;
        public int[] like_range;
        public float retweet_probability;
        public float backlash_probability;
        public float suspension_risk;
        public string[] messages;
    }

    [Serializable]
    public class GlobalSettings
    {
        public int base_follower_gain;
        public float flame_gauge_threshold;
        public float suspension_threshold;
        public float combo_multiplier;
        public int time_limit_seconds;
    }
}