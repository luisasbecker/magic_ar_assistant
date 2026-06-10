using System;

namespace MagicARAssistant.Data
{
    [Serializable]
    public sealed class MarkerData
    {
        public string id;
        public string label;
        public string effectDescription;
        public int value;
        public string markerType;
        public string referenceImageName;
    }
}

