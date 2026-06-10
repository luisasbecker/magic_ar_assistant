using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MagicARAssistant.Utils;
using UnityEngine;

namespace MagicARAssistant.Data
{
    [Serializable]
    public sealed class CardDataCollection
    {
        public List<CardData> cards = new();
    }

    [Serializable]
    public sealed class MarkerDataCollection
    {
        public List<MarkerData> markers = new();
    }

    public sealed class DataRepository
    {
        public IReadOnlyList<CardData> Cards => _cards;
        public IReadOnlyList<MarkerData> Markers => _markers;

        private readonly List<CardData> _cards = new();
        private readonly List<MarkerData> _markers = new();

        public void Load()
        {
            _cards.Clear();
            _markers.Clear();

            if (!TryLoadStreamingJson("Data/cards.json", out CardDataCollection cardCollection) || cardCollection.cards.Count == 0)
            {
                cardCollection = CreateDefaultCards();
            }

            if (!TryLoadStreamingJson("Data/markers.json", out MarkerDataCollection markerCollection) || markerCollection.markers.Count == 0)
            {
                markerCollection = CreateDefaultMarkers();
            }

            _cards.AddRange(cardCollection.cards);
            _markers.AddRange(markerCollection.markers);
        }

        public CardData GetCardById(string id)
        {
            return _cards.FirstOrDefault(card => string.Equals(card.id, id, StringComparison.OrdinalIgnoreCase));
        }

        public CardData GetCardByReferenceImage(string referenceImageName)
        {
            return _cards.FirstOrDefault(card => string.Equals(card.referenceImageName, referenceImageName, StringComparison.OrdinalIgnoreCase));
        }

        public MarkerData GetMarkerById(string id)
        {
            return _markers.FirstOrDefault(marker => string.Equals(marker.id, id, StringComparison.OrdinalIgnoreCase));
        }

        public MarkerData GetMarkerByReferenceImage(string referenceImageName)
        {
            return _markers.FirstOrDefault(marker => string.Equals(marker.referenceImageName, referenceImageName, StringComparison.OrdinalIgnoreCase));
        }

        private static bool TryLoadStreamingJson<T>(string relativePath, out T value)
        {
            value = default;
            try
            {
                string path = Path.Combine(Application.streamingAssetsPath, relativePath);
                if (!File.Exists(path))
                {
                    return false;
                }

                string json = File.ReadAllText(path);
                return JsonUtils.TryFromJson(json, out value);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Falha ao carregar dados locais '{relativePath}': {exception.Message}");
                return false;
            }
        }

        private static CardDataCollection CreateDefaultCards()
        {
            return new CardDataCollection
            {
                cards = new List<CardData>
                {
                    new()
                    {
                        id = "CARD_CREATURE_001",
                        name = "Guardiao Arcano",
                        category = "Carta",
                        manaCost = "2U",
                        typeLine = "Criatura - Guardiao",
                        rulesText = "Quando entra em campo, olhe o topo do grimorio ficticio.",
                        power = "2",
                        toughness = "3",
                        defaultCounters = 0,
                        notes = "Carta ficticia criada para o prototipo academico.",
                        referenceImageName = "CARD_CREATURE_001"
                    },
                    new()
                    {
                        id = "CARD_LAND_001",
                        name = "Floresta Antiga",
                        category = "Carta",
                        manaCost = "",
                        typeLine = "Terreno",
                        rulesText = "Gera um recurso verde ficticio.",
                        power = "",
                        toughness = "",
                        defaultCounters = 0,
                        notes = "Sem relacao com cartas oficiais.",
                        referenceImageName = "CARD_LAND_001"
                    },
                    new()
                    {
                        id = "CARD_SPELL_001",
                        name = "Raio Mistico",
                        category = "Carta",
                        manaCost = "1R",
                        typeLine = "Feitico",
                        rulesText = "Causa 2 pontos de dano ficticio a um alvo.",
                        power = "",
                        toughness = "",
                        defaultCounters = 0,
                        notes = "Texto e nome ficticios.",
                        referenceImageName = "CARD_SPELL_001"
                    },
                    new()
                    {
                        id = "CARD_COMMANDER_001",
                        name = "Comandante das Chamas",
                        category = "Carta",
                        manaCost = "3RR",
                        typeLine = "Criatura Lendaria - Comandante",
                        rulesText = "Sempre que um marcador for aplicado, registre uma entrada adicional no log.",
                        power = "4",
                        toughness = "4",
                        defaultCounters = 0,
                        notes = "Comandante ficticio para demonstracao.",
                        referenceImageName = "CARD_COMMANDER_001"
                    }
                }
            };
        }

        private static MarkerDataCollection CreateDefaultMarkers()
        {
            return new MarkerDataCollection
            {
                markers = new List<MarkerData>
                {
                    new()
                    {
                        id = "MARKER_PLUS_ONE",
                        label = "Marcador +1/+1",
                        effectDescription = "Incrementa o contador +1/+1 da carta selecionada.",
                        value = 1,
                        markerType = "PLUS_ONE",
                        referenceImageName = "MARKER_PLUS_ONE"
                    },
                    new()
                    {
                        id = "MARKER_DAMAGE",
                        label = "Marcador de Dano",
                        effectDescription = "Adiciona dano acumulado a carta selecionada.",
                        value = 1,
                        markerType = "DAMAGE",
                        referenceImageName = "MARKER_DAMAGE"
                    },
                    new()
                    {
                        id = "MARKER_ABILITY",
                        label = "Marcador de Habilidade",
                        effectDescription = "Registra uma habilidade adicional ficticia na carta.",
                        value = 1,
                        markerType = "ABILITY",
                        referenceImageName = "MARKER_ABILITY"
                    },
                    new()
                    {
                        id = "MARKER_LIFE_GAIN",
                        label = "Marcador de Ganho de Vida",
                        effectDescription = "Sugere ganho de vida ao jogador ativo.",
                        value = 1,
                        markerType = "LIFE_GAIN",
                        referenceImageName = "MARKER_LIFE_GAIN"
                    }
                }
            };
        }
    }
}

