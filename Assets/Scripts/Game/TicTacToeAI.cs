using System;
using System.Collections.Generic;
using static Omok.Constants;
using Random = System.Random;
using UnityEngine;

namespace Omok
{

    public static class TicTacToeAI
    {

        const int WIN_SCORE = 10000000;
        static Random rand = new();

        public static (int x, int y)? GetBestMove(PlayerType[,] board, PlayerType player, int kyu)
        {
            kyu = Math.Clamp(kyu, 1, 18);

            var settings = GetLevelSettings(kyu);
            PlayerType opponent = Opp(player);

            // 이기는 수 있으면 바로 둠
            var winMove = FindWinningMove(board, player);
            if (winMove.x != -1)
                return winMove;

            // 상대 바로 이기는 수 막기
            var blockMove = FindWinningMove(board, opponent);
            if (blockMove.x != -1)
                return blockMove;

            (int x, int y) bestMove = (-1, -1);

            int bestScore = int.MinValue;
            var moves = GenerateMoves(board, kyu);

            foreach (var move in moves)
            {
                board[move.y, move.x] = player;

                int score = AlphaBeta(
                    board,
                    settings.Depth,
                    int.MinValue,
                    int.MaxValue,
                    false,
                    player,
                    settings);

                board[move.y, move.x] = PlayerType.None;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        public static int Remap(int value, int inMin, int inMax, int outMin, int outMax)
        {
            return (int)((value - inMin) * (outMax - outMin) / (float)(inMax - inMin) + outMin);
        }

        public static float Remap(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return (value - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
        }

        static readonly (int dx, int dy)[] dirs =
        {
            (1, 0), (0, 1), (1, 1), (1, -1)
        };

        struct LevelSettings
        {

            public int Kyu;
            public int Depth;
            public float EvalAccuracy;
            public float Randomness;

        }

        static LevelSettings GetLevelSettings(int kyu)
        {
            return new LevelSettings
            {
                Kyu = Math.Clamp(kyu, 1, 18),
                Depth = Remap(19 - kyu, 1, 18, 1, 1),
                EvalAccuracy = Remap(19 - kyu, 1, 18, 0.4f, 1f),
                Randomness = (kyu-1) * 0.02f
            };
        }

        static int AlphaBeta(
            PlayerType[,] board,
            int depth,
            int alpha,
            int beta,
            bool maximizing,
            PlayerType me,
            LevelSettings settings)
        {
            PlayerType opp = Opp(me);

            if (depth == 0 || CheckGameWin(me, board) || CheckGameWin(opp, board))
                return Evaluate(board, me, settings);

            var moves = GenerateMoves(board, settings.Kyu);

            if (maximizing)
            {
                int value = int.MinValue;

                foreach (var m in moves)
                {
                    board[m.y, m.x] = me;

                    value = Math.Max(value, AlphaBeta(board, depth - 1, alpha, beta, false, me, settings));

                    board[m.y, m.x] = PlayerType.None;

                    alpha = Math.Max(alpha, value);
                    if (beta <= alpha) break;
                }

                return value;
            }
            else
            {
                int value = int.MaxValue;

                foreach (var m in moves)
                {
                    board[m.y, m.x] = opp;

                    value = Math.Min(value, AlphaBeta(board, depth - 1, alpha, beta, true, me, settings));

                    board[m.y, m.x] = PlayerType.None;

                    beta = Math.Min(beta, value);
                    if (beta <= alpha) break;
                }

                return value;
            }
        }

        static List<(int x, int y)> GenerateMoves(PlayerType[,] board, int kyu)
        {
            var list = new List<(int, int)>();
            bool hasStone = false;

            for (int y = 0; y < BOARD_SIZE; y++)
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                if (board[y, x] != PlayerType.None)
                {
                    hasStone = true;

                    for (int dy = -2; dy <= 2; dy++)
                    for (int dx = -2; dx <= 2; dx++)
                    {
                        int nx = x + dx, ny = y + dy;
                        if (nx < 0 || ny < 0 || nx >= BOARD_SIZE || ny >= BOARD_SIZE) continue;

                        if (board[ny, nx] == PlayerType.None && !list.Contains((nx, ny)))
                            list.Add((nx, ny));
                    }
                }
            }

            if (!hasStone)
                list.Add((BOARD_SIZE / 2, BOARD_SIZE / 2));

            list.Shuffle(); // 돌을 놓는 위치 랜덤하게 적용하여 항상 같은 수를 놓지 않도록 함
            if (kyu > 0)
                list.RemoveRandomRatio((kyu - 1) / 200f); // 급수별로 임의로 놓치는 수를 만듬.(낮은 급수일수록 더 많은 수를 놓침)
            return list;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]); // swap
            }
        }

        public static void RemoveRandomRatio<T>(this IList<T> list, float ratio)
        {
            if (list == null)
                return;

            if (ratio < 0 || ratio > 1)
                return;

            int removeCount = (int)Math.Round(list.Count * ratio);
            for (int i = 0; i < removeCount; i++)
            {
                int index = rand.Next(list.Count);
                list.RemoveAt(index);
            }
        }

        static int Evaluate(PlayerType[,] board, PlayerType me, LevelSettings settings)
        {
            PlayerType opp = Opp(me);

            int myScore = ScoreFor(board, me);
            int oppScore = ScoreFor(board, opp);

            int result = myScore - oppScore;

            // result = (int)(result * settings.EvalAccuracy);
            //
            // if (settings.Randomness > 0)
            //     result += (int)((rand.NextDouble() - 0.5) * 2000 * settings.Randomness);

            return result;
        }

        static int ScoreFor(PlayerType[,] board, PlayerType p)
        {
            int score = 0;

            for (int y = 0; y < BOARD_SIZE; y++)
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                if (board[y, x] != p) continue;

                foreach (var d in dirs)
                    score += EvaluateDir(board, x, y, d.dx, d.dy, p);
            }

            return score;
        }

        static int EvaluateDir(PlayerType[,] board, int x, int y, int dx, int dy, PlayerType p)
        {
            int count = 0;
            int open = 0;

            int i = 1;
            while (true)
            {
                int nx = x + dx * i;
                int ny = y + dy * i;
                if (nx < 0 || ny < 0 || nx >= BOARD_SIZE || ny >= BOARD_SIZE) break;

                if (board[ny, nx] == p) count++;
                else
                {
                    if (board[ny, nx] == PlayerType.None) open++;
                    break;
                }

                i++;
            }

            i = 1;
            while (true)
            {
                int nx = x - dx * i;
                int ny = y - dy * i;
                if (nx < 0 || ny < 0 || nx >= BOARD_SIZE || ny >= BOARD_SIZE) break;

                if (board[ny, nx] == p) count++;
                else
                {
                    if (board[ny, nx] == PlayerType.None) open++;
                    break;
                }

                i++;
            }

            if (count >= 4) return WIN_SCORE;
            if (count == 3 && open == 2) return 100000;
            if (count == 3 && open == 1) return 10000;
            if (count == 2 && open == 2) return 5000;
            if (count == 2 && open == 1) return 500;
            if (count == 1 && open == 2) return 100;

            return 10;
        }

        static PlayerType Opp(PlayerType p)
            => p == PlayerType.Player1 ? PlayerType.Player2 : PlayerType.Player1;

        static (int x, int y) FindWinningMove(PlayerType[,] board, PlayerType player)
        {
            var moves = GenerateMoves(board, -1); // 모든 가능한 수 생성

            foreach (var m in moves)
            {
                board[m.y, m.x] = player;

                if (CheckGameWin(player, board))
                {
                    board[m.y, m.x] = PlayerType.None;
                    return m;
                }

                board[m.y, m.x] = PlayerType.None;
            }

            return (-1, -1);
        }

        public static bool CheckGameWin(PlayerType playerType, PlayerType[,] board)
        {
            // 가로 체크
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col <= BOARD_SIZE - 5; col++)
                {
                    if (board[row, col] == playerType &&
                        board[row, col + 1] == playerType &&
                        board[row, col + 2] == playerType &&
                        board[row, col + 3] == playerType &&
                        board[row, col + 4] == playerType)
                        return true;
                }
            }

            // 세로 체크
            for (int col = 0; col < BOARD_SIZE; col++)
            {
                for (int row = 0; row <= BOARD_SIZE - 5; row++)
                {
                    if (board[row, col] == playerType &&
                        board[row + 1, col] == playerType &&
                        board[row + 2, col] == playerType &&
                        board[row + 3, col] == playerType &&
                        board[row + 4, col] == playerType)
                        return true;
                }
            }

            // 대각선 체크 (↘)
            for (int row = 0; row <= BOARD_SIZE - 5; row++)
            {
                for (int col = 0; col <= BOARD_SIZE - 5; col++)
                {
                    if (board[row, col] == playerType &&
                        board[row + 1, col + 1] == playerType &&
                        board[row + 2, col + 2] == playerType &&
                        board[row + 3, col + 3] == playerType &&
                        board[row + 4, col + 4] == playerType)
                        return true;
                }
            }

            // 대각선 체크 (↙)
            for (int row = 0; row <= BOARD_SIZE - 5; row++)
            {
                for (int col = 4; col < BOARD_SIZE; col++)
                {
                    if (board[row, col] == playerType &&
                        board[row + 1, col - 1] == playerType &&
                        board[row + 2, col - 2] == playerType &&
                        board[row + 3, col - 3] == playerType &&
                        board[row + 4, col - 4] == playerType)
                        return true;
                }
            }

            return false;
        }

        public static bool CheckGameDraw(PlayerType[,] board)
        {
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    if (board[row, col] == PlayerType.None)
                        return false;
                }
            }

            return true;
        }

    }

}