using System.Collections.Generic;
using static Omok.Constants;

namespace Game
{
    public class RenjuRuleChecker
    {
        private static readonly int[] dx = { 1, 0, 1, 1 };
        private static readonly int[] dy = { 0, 1, 1, -1 };

        public static List<(int x, int y)> GetForbiddenMoves(PlayerType[,] board, PlayerType player)
        {
            List<(int x, int y)> forbiddenList = new List<(int x, int y)>();

            if (player != PlayerType.Player1) return forbiddenList;

            int size = board.GetLength(0);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (board[x, y] == PlayerType.None && IsForbidden(board, x, y, player))
                    {
                        forbiddenList.Add((x, y));
                    }
                }
            }
            return forbiddenList;
        }

        public static bool IsForbidden(PlayerType[,] board, int x, int y, PlayerType player)
        {
            if (player != PlayerType.Player1) return false;

            // 1. 장목 (6목 이상)
            if (CheckOverline(board, x, y, player)) return true;

            // 2. 44 금수
            if (CountFours(board, x, y, player) >= 2) return true;

            // 3. 33 금수
            if (CountOpenThrees(board, x, y, player) >= 2) return true;

            return false;
        }

        private static bool CheckOverline(PlayerType[,] board, int x, int y, PlayerType player)
        {
            for (int i = 0; i < 4; i++)
            {
                if (GetLineLength(board, x, y, dx[i], dy[i], player) >= 6) return true;
            }
            return false;
        }

        private static int CountFours(PlayerType[,] board, int x, int y, PlayerType player)
        {
            int fourCount = 0;
            for (int i = 0; i < 4; i++)
            {
                if (IsOpenFour(board, x, y, dx[i], dy[i], player)) fourCount++;
            }
            return fourCount;
        }

        private static int CountOpenThrees(PlayerType[,] board, int x, int y, PlayerType player)
        {
            int threeCount = 0;
            for (int i = 0; i < 4; i++)
            {
                if (IsOpenThree(board, x, y, dx[i], dy[i], player)) threeCount++;
            }
            return threeCount;
        }

        private static int GetLineLength(PlayerType[,] board, int x, int y, int dX, int dY, PlayerType player)
        {
            int count = 1;
            count += CountContinuous(board, x, y, dX, dY, player);
            count += CountContinuous(board, x, y, -dX, -dY, player);
            return count;
        }

        private static int CountContinuous(PlayerType[,] board, int x, int y, int dX, int dY, PlayerType player)
        {
            int count = 0;
            int nx = x + dX;
            int ny = y + dY;
            int size = board.GetLength(0);

            while (nx >= 0 && nx < size && ny >= 0 && ny < size && board[nx, ny] == player)
            {
                count++;
                nx += dX;
                ny += dY;
            }
            return count;
        }

        private static bool IsOpenFour(PlayerType[,] board, int x, int y, int dX, int dY, PlayerType player)
        {
            int count = 1;
            count += CountContinuous(board, x, y, dX, dY, player);
            count += CountContinuous(board, x, y, -dX, -dY, player);

            if (count != 4) return false;

            int forwardX = x + dX * (CountContinuous(board, x, y, dX, dY, player) + 1);
            int forwardY = y + dY * (CountContinuous(board, x, y, dX, dY, player) + 1);
            int backwardX = x - dX * (CountContinuous(board, x, y, -dX, -dY, player) + 1);
            int backwardY = y - dY * (CountContinuous(board, x, y, -dX, -dY, player) + 1);

            return IsInBoard(board, forwardX, forwardY) && board[forwardX, forwardY] == PlayerType.None &&
                   IsInBoard(board, backwardX, backwardY) && board[backwardX, backwardY] == PlayerType.None;
        }

        private static bool IsOpenThree(PlayerType[,] board, int x, int y, int dX, int dY, PlayerType player)
        {
            int count = 1;
            count += CountContinuous(board, x, y, dX, dY, player);
            count += CountContinuous(board, x, y, -dX, -dY, player);

            if (count != 3) return false;

            int forwardX = x + dX * (CountContinuous(board, x, y, dX, dY, player) + 1);
            int forwardY = y + dY * (CountContinuous(board, x, y, dX, dY, player) + 1);
            int backwardX = x - dX * (CountContinuous(board, x, y, -dX, -dY, player) + 1);
            int backwardY = y - dY * (CountContinuous(board, x, y, -dX, -dY, player) + 1);

            bool forwardOpenFour = IsInBoard(board, forwardX, forwardY) &&
                                   board[forwardX, forwardY] == PlayerType.None &&
                                   IsOpenFour(board, forwardX, forwardY, dX, dY, player);

            bool backwardOpenFour = IsInBoard(board, backwardX, backwardY) &&
                                    board[backwardX, backwardY] == PlayerType.None &&
                                    IsOpenFour(board, backwardX, backwardY, dX, dY, player);

            return forwardOpenFour || backwardOpenFour;
        }

        private static bool IsInBoard(PlayerType[,] board, int x, int y)
            => x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1);
    }
}
