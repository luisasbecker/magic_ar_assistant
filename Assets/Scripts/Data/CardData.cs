using System;

namespace MagicARAssistant.Data
{
    [Serializable]
    public sealed class CardData
    {
        public string id;
        public string name;
        public string category;
        public string manaCost;
        public string typeLine;
        public string rulesText;
        public string power;
        public string toughness;
        public int defaultCounters;
        public string notes;
        public string referenceImageName;
    }
}

