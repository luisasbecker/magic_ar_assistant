using System.Linq;
using MagicARAssistant.Data;
using MagicARAssistant.Match;
using NUnit.Framework;

namespace MagicARAssistant.Tests
{
    public sealed class MatchServicesTests
    {
        private MatchLogService _log;
        private UndoService _undo;
        private MatchStateManager _stateManager;
        private LifeCounterController _life;
        private TurnPhaseController _turn;
        private MarkerApplicationService _markers;

        [SetUp]
        public void SetUp()
        {
            _log = new MatchLogService();
            _undo = new UndoService();
            _stateManager = new MatchStateManager(_log);
            _life = new LifeCounterController(_stateManager, _undo, _log);
            _turn = new TurnPhaseController(_stateManager, _undo, _log);
            _markers = new MarkerApplicationService(_stateManager, _undo, _log);
            _stateManager.StartNewMatch();
        }

        [Test]
        public void VidaInicialDeveSer20ParaAmbosJogadores()
        {
            Assert.AreEqual(20, _stateManager.State.playerALife);
            Assert.AreEqual(20, _stateManager.State.playerBLife);
        }

        [Test]
        public void MaisUmAumentaVidaCorretamente()
        {
            _life.ChangeLife("Jogador A", 1);
            Assert.AreEqual(21, _stateManager.State.playerALife);
        }

        [Test]
        public void MenosUmReduzVidaCorretamente()
        {
            _life.ChangeLife("Jogador A", -1);
            Assert.AreEqual(19, _stateManager.State.playerALife);
        }

        [Test]
        public void ResetVoltaVidaPara20()
        {
            _life.ChangeLife("Jogador A", 5);
            _life.ResetLife("Jogador A");
            Assert.AreEqual(20, _stateManager.State.playerALife);
        }

        [Test]
        public void DesfazerRestauraValorAnterior()
        {
            _life.ChangeLife("Jogador A", 5);
            Assert.IsTrue(_undo.TryUndo(out _));
            Assert.AreEqual(20, _stateManager.State.playerALife);
        }

        [Test]
        public void ProximaFaseAvancaCorretamente()
        {
            _turn.NextPhase();
            Assert.AreEqual("Principal 1", _stateManager.State.phase);
        }

        [Test]
        public void PassarTurnoAlternaJogadorAtivo()
        {
            _turn.PassTurn();
            Assert.AreEqual("Jogador B", _stateManager.State.activePlayer);
            Assert.AreEqual(1, _stateManager.State.turnNumber);
            _turn.PassTurn();
            Assert.AreEqual("Jogador A", _stateManager.State.activePlayer);
            Assert.AreEqual(2, _stateManager.State.turnNumber);
        }

        [Test]
        public void AplicarMarcadorPlusOneIncrementaContador()
        {
            DetectedCardState card = AddCard();
            _markers.ApplyMarker(card, PlusOne());
            Assert.AreEqual(1, card.countersPlusOne);
        }

        [Test]
        public void RemoverMarcadorDecrementaSemFicarNegativo()
        {
            DetectedCardState card = AddCard();
            _markers.RemoveMarker(card, PlusOne());
            Assert.AreEqual(0, card.countersPlusOne);
        }

        [Test]
        public void SalvarECarregarJsonPreservaEstado()
        {
            LocalJsonStorage storage = new();
            _life.ChangeLife("Jogador B", -5);
            Assert.IsTrue(storage.SaveMatch(_stateManager.State, out string saveError), saveError);
            Assert.IsTrue(storage.LoadLastMatch(out MatchState loaded, out string loadError), loadError);
            Assert.AreEqual(15, loaded.playerBLife);
        }

        [Test]
        public void LogRegistraAlteracaoDeVida()
        {
            _life.ChangeLife("Jogador A", 1);
            Assert.IsTrue(_stateManager.State.eventLog.Any(entry => entry.eventType == "Vida alterada"));
        }

        [Test]
        public void LogRegistraAplicacaoDeMarcador()
        {
            DetectedCardState card = AddCard();
            _markers.ApplyMarker(card, PlusOne());
            Assert.IsTrue(_stateManager.State.eventLog.Any(entry => entry.eventType == "Marcador aplicado"));
        }

        private DetectedCardState AddCard()
        {
            CardData cardData = new()
            {
                id = "CARD_CREATURE_001",
                name = "Guardiao Arcano",
                defaultCounters = 0,
                referenceImageName = "CARD_CREATURE_001"
            };
            DetectedCardState card = _stateManager.AddOrUpdateDetectedCard(cardData, "test-instance", "Manual", true);
            _stateManager.SelectCard(card.instanceId);
            return card;
        }

        private static MarkerData PlusOne()
        {
            return new MarkerData
            {
                id = "MARKER_PLUS_ONE",
                label = "Marcador +1/+1",
                markerType = "PLUS_ONE",
                value = 1
            };
        }
    }
}

