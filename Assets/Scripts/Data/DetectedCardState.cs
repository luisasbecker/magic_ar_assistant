using System;
using System.Collections.Generic;

namespace MagicARAssistant.Data
{
    [Serializable]
    public sealed class DetectedCardState
    {
        public string instanceId;
        public string cardId;
        public string displayName;
        public int countersPlusOne;
        public int damageCounters;
        public List<string> abilityMarkers = new();
        public bool isPinned;
        public bool isVisible = true;
        public string lastTrackingState = "None";
    }
}

