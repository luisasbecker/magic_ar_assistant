using System;

namespace MagicARAssistant.Data
{
    [Serializable]
    public sealed class MatchLogEntry
    {
        public string timestamp;
        public string eventType;
        public string description;
        public string relatedCardInstanceId;
        public string oldValue;
        public string newValue;
    }
}

