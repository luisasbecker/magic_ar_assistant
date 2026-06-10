using System;
using MagicARAssistant.Data;
using MagicARAssistant.Utils;

namespace MagicARAssistant.Match
{
    public sealed class MatchLogService
    {
        public event Action<MatchLogEntry> LogAdded;

        public MatchLogEntry Add(MatchState state, string eventType, string description, string relatedCardInstanceId = "", string oldValue = "", string newValue = "")
        {
            if (state == null)
            {
                return null;
            }

            MatchLogEntry entry = new()
            {
                timestamp = DateTimeUtils.NowIsoUtc(),
                eventType = eventType,
                description = description,
                relatedCardInstanceId = relatedCardInstanceId,
                oldValue = oldValue,
                newValue = newValue
            };

            state.eventLog.Add(entry);
            state.updatedAt = entry.timestamp;
            LogAdded?.Invoke(entry);
            return entry;
        }
    }
}

