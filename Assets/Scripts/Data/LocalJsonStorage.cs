using System;
using System.IO;
using MagicARAssistant.Utils;
using UnityEngine;

namespace MagicARAssistant.Data
{
    public sealed class LocalJsonStorage
    {
        public string LastMatchPath => Path.Combine(Application.persistentDataPath, "last_match.json");

        public bool SaveMatch(MatchState state, out string error)
        {
            error = string.Empty;
            if (state == null)
            {
                error = "Estado da partida esta vazio.";
                return false;
            }

            try
            {
                Directory.CreateDirectory(Application.persistentDataPath);
                state.updatedAt = DateTimeUtils.NowIsoUtc();
                File.WriteAllText(LastMatchPath, JsonUtils.ToPrettyJson(state));
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        public bool LoadLastMatch(out MatchState state, out string error)
        {
            state = null;
            error = string.Empty;

            try
            {
                if (!File.Exists(LastMatchPath))
                {
                    error = "Nenhuma partida salva encontrada.";
                    return false;
                }

                string json = File.ReadAllText(LastMatchPath);
                if (!JsonUtils.TryFromJson(json, out state))
                {
                    error = "Arquivo de partida salvo esta invalido.";
                    return false;
                }

                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        public bool ResetLastMatch(out string error)
        {
            error = string.Empty;
            try
            {
                if (File.Exists(LastMatchPath))
                {
                    File.Delete(LastMatchPath);
                }

                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        public bool ExportLogJson(MatchState state, out string path, out string error)
        {
            path = Path.Combine(Application.persistentDataPath, $"match_log_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json");
            error = string.Empty;
            try
            {
                File.WriteAllText(path, JsonUtils.ToPrettyJson(state));
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        public bool ExportLogText(MatchState state, out string path, out string error)
        {
            path = Path.Combine(Application.persistentDataPath, $"match_log_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt");
            error = string.Empty;
            try
            {
                using StreamWriter writer = new(path);
                writer.WriteLine("Magic AR Assistant - Log da Partida");
                writer.WriteLine($"Criada em: {state.createdAt}");
                writer.WriteLine($"Atualizada em: {state.updatedAt}");
                writer.WriteLine();
                foreach (MatchLogEntry entry in state.eventLog)
                {
                    writer.WriteLine($"[{entry.timestamp}] {entry.eventType}: {entry.description} ({entry.oldValue} -> {entry.newValue})");
                }

                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }
    }
}

