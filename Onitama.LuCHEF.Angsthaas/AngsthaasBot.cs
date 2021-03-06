﻿using System;
using System.Linq;
using System.Threading;
using Onitama.LuCHEF.Angsthaas.Server;
using RemoteBotClient;

namespace Onitama.LuCHEF.Angsthaas
{
    public class AngsthaasBot
    {
        private readonly IBotInterface _botProxy;

        public AngsthaasBot(IBotInterface botProxy)
        {
            _botProxy = botProxy;
        }

        public void RunGameLoop(CancellationToken cancellationToken)
        {
            var gameInfo = _botProxy.Read<GameInfo>();

            _botProxy.Log($"Playing as {gameInfo.Identity}");

            while(true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var state = _botProxy.Read<GameState>();

                _botProxy.Log(Environment.NewLine + CondensedUtf8Formatter.Instance.Format(state));

                Move move = DetermineMove(state);

                _botProxy.Log($"Moving {move.UsedCard}! Run away, run away!!\n");

                _botProxy.Write(move);
            }
        }

        private Move DetermineMove(GameState state)
        {
            var myPieces = state.Pieces.Where(p => p.Owner == state.CurrentlyPlaying);

            Move bestMove = null;
            int bestDistance = 0;

            foreach (var piece in myPieces)
            {
                foreach (var card in state.MyHand)
                {
                    foreach (var target in card.Targets)
                    {
                        var targetPosition = piece.PositionOnBoard + target;

                        if (!IsValidMove(state, targetPosition))
                        {
                            continue;
                        }

                        var move = new PlayMove(
                            usedCard: card.Type,
                            from: piece.PositionOnBoard,
                            to: targetPosition
                        );

                        if (IsWinningMove(state, move))
                        {
                            _botProxy.Log("Wow, we found a winning move!?");
                            return move;
                        }

                        var position = GetPositionAfterMove(state, move);

                        var distance = CalculateSummedManhattanDistance(position);

                        if (distance > bestDistance)
                        {
                            bestMove = move;
                            bestDistance = distance;
                        }
                    }
                }
            }

            return bestMove ?? new PassMove(state.MyHand.First().Type);
        }

        private static bool IsWinningMove(GameState state, PlayMove move)
        {
            var targetPiece = state.Pieces.FirstOrDefault(p => p.PositionOnBoard == move.To);

            if (targetPiece != null 
                && targetPiece.Owner != state.CurrentlyPlaying 
                && targetPiece.IsMaster)
            {
                return true;
            }

            var targetBase = state.CurrentlyPlaying == PlayerIdentity.Player1
                ? Position.Player2Home
                : Position.Player1Home;

            if (move.To == targetBase)
            {
                return true;
            }

            return false;
        }

        private static bool IsValidMove(GameState state, Position targetPosition)
        {
            if (targetPosition.IsOutOfBounds)
            {
                return false;
            }

            var pieceOnTargetPosition = state.Pieces.FirstOrDefault(p => p.PositionOnBoard == targetPosition);

            if (pieceOnTargetPosition?.Owner == state.CurrentlyPlaying)
            {
                return false;
            }

            return true;
        }

        private static BoardPosition GetPositionAfterMove(BoardPosition boardPosition, PlayMove move)
        {
            var clone = BoardPosition.Clone(boardPosition);

            var piece = clone.Pieces.Single(p => p.PositionOnBoard == move.From);
            piece.PositionOnBoard = move.To;

            return clone;
        }

        private static int CalculateSummedManhattanDistance(BoardPosition state)
        {
            var set1 = state.Pieces.Where(p => p.Owner == PlayerIdentity.Player1).ToArray();
            var set2 = state.Pieces.Where(p => p.Owner == PlayerIdentity.Player2).ToArray();
            int sum = 0;

            for (int i = 0; i < set1.Length; i++)
            {
                for (int j = i; j < set2.Length; j++)
                {
                    sum += CalculateManhattanDistance(set1[i].PositionOnBoard, set2[j].PositionOnBoard);
                }
            }

            return sum;
        }

        private static int CalculateManhattanDistance(Position p1, Position p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }
    }
}
