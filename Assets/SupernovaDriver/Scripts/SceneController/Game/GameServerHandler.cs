using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elympics;
using SupernovaDriver.Scripts.Utilities;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game
{
    public class GameServerHandler : ElympicsMonoBehaviour, IServerHandlerGuid
    {
        private static readonly TimeSpan StartGameTimeout = TimeSpan.FromSeconds(30);

        private int      _playersNumber;
        private DateTime _waitToStartFinishTime;
        private bool     _gameStarted;

        private readonly HashSet<ElympicsPlayer> _playersConnected = new();

        public void OnServerInit(InitialMatchPlayerDatasGuid initialMatchPlayerDatas)
        {
            if (!IsEnabledAndActive)
                return;

            _playersNumber = initialMatchPlayerDatas.Count;
            var humansPlayers = initialMatchPlayerDatas.Count(x => !x.IsBot);
            DLogger.Log($"Game initialized for {initialMatchPlayerDatas.Count} players "
                        + $"(including {initialMatchPlayerDatas.Count - humansPlayers} bots).");
            DLogger.Log($"Waiting for {humansPlayers} human players to connect.");
            var sb = new StringBuilder()
                .AppendLine($"MatchId: {initialMatchPlayerDatas.MatchId}")
                .AppendLine($"QueueName: {initialMatchPlayerDatas.QueueName}")
                .AppendLine($"RegionName: :{initialMatchPlayerDatas.RegionName}")
                .AppendLine(
                    $"CustomMatchmakingData: {initialMatchPlayerDatas.CustomMatchmakingData?.Count.ToString() ?? "null"}")
                .AppendLine(
                    $"CustomRoomData: {(initialMatchPlayerDatas.CustomRoomData != null ? string.Join(", ", initialMatchPlayerDatas.CustomRoomData.Select(x => x.Key)) : "null")}")
                .AppendLine(
                    $"ExternalGameData: {initialMatchPlayerDatas.ExternalGameData?.Length.ToString() ?? "null"}");

            foreach (var playerData in initialMatchPlayerDatas)
                _ = sb.AppendLine(
                    $"Player {playerData.UserId} {(playerData.IsBot ? "Bot" : "Human")} room {playerData.RoomId} teamIndex {playerData.TeamIndex}");
            DLogger.Log(sb.ToString());

            _ = StartCoroutine(WaitForGameStartOrEnd());
        }

        private IEnumerator WaitForGameStartOrEnd()
        {
            _waitToStartFinishTime = DateTime.Now + StartGameTimeout;

            while (DateTime.Now < _waitToStartFinishTime)
            {
                if (_gameStarted)
                    yield break;

                DLogger.Log("Not all players connected yet...");
                yield return new WaitForSeconds(5);
            }

            DLogger.LogWarning("Forcing game server to quit because some players did not connect on time.\n"
                               + "Connected players: "
                               + string.Join(", ", _playersConnected));
            Elympics.EndGame();
        }

        public void OnPlayerDisconnected(ElympicsPlayer player)
        {
            if (!IsEnabledAndActive)
                return;

            DLogger.Log($"Player {player} disconnected.");
            DLogger.LogWarning("Forcing game server to quit because one of the players disconnected.");
            Elympics.EndGame();
        }

        public void OnPlayerConnected(ElympicsPlayer player)
        {
            if (!IsEnabledAndActive)
                return;

            DLogger.Log($"Player {player} connected.");

            _ = _playersConnected.Add(player);
            if (_playersConnected.Count != _playersNumber || _gameStarted)
                return;

            _gameStarted = true;
            GameController.Instance.StartGame(false);

            DLogger.Log("All players have connected.");
        }
    }
}