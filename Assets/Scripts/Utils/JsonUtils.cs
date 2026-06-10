using UnityEngine;

namespace MagicARAssistant.Utils
{
    public static class JsonUtils
    {
        public static string ToPrettyJson<T>(T value)
        {
            return JsonUtility.ToJson(value, true);
        }

        public static bool TryFromJson<T>(string json, out T value)
        {
            value = default;
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            try
            {
                value = JsonUtility.FromJson<T>(json);
                return value != null;
            }
            catch
            {
                return false;
            }
        }
    }
}

