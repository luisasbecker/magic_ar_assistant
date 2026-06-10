using System;
using System.Collections.Generic;

namespace MagicARAssistant.Data
{
    [Serializable]
    public sealed class MatchState
    {
        public int playerALife = 20;
        public int playerBLife = 20;
        public int turnNumber = 1;
        public string activePlayer = "Jogador A";
        public string phase = "Inicio";
        public List<DetectedCardState> detectedCards = new();
        public List<string> appliedMarkers = new();
        public List<MatchLogEntry> eventLog = new();
        public string createdAt;
        public string updatedAt;
    }
}

