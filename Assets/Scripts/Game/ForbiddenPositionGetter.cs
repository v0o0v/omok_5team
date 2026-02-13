using System;
using System.Collections.Generic;
using static Omok.Constants;

namespace Game
{
    public class ForbiddenPositionGetter
    {
        // 가로(x), 세로(y) 기준 방향 벡터
        // 순서대로: 가로, 세로, 우하향 대각선, 우상향 대각선
        private static readonly int[] dx = { 1, 0, 1, 1 };
        private static readonly int[] dy = { 0, 1, 1, -1 };

        public List<(int x, int y)> GetForbiddenPosition(PlayerType[,] board, PlayerType player)
        {
            List<(int x, int y)> forbiddenList = new List<(int x, int y)>();

            if (player != PlayerType.Player1) return forbiddenList;

            int width = board.GetLength(0);  // 가로 크기
            int height = board.GetLength(1); // 세로 크기

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // 빈 공간이고 금수라면 리스트에 추가
                    if (board[x, y] == PlayerType.None && IsForbidden(board, x, y, player))
                    {
                        // UI에서 PutForbiddenMark(y, x)로 호출했을 때 잘 나왔으므로
                        // 여기서부터 (y, x) 순서로 튜플을 만들어 넘겨줍니다.
                        forbiddenList.Add((y, x));
                    }
                }
            }
            return forbiddenList;
        }

        public static bool IsForbidden(PlayerType[,] board, int x, int y, PlayerType player)
        {
            // 1. 임시 착수 (가상 시뮬레이션)
            board[x, y] = player;

            try
            {
                // 2. 장목 (6목 이상)
                if (CheckOverline(board, x, y, player)) return true;

                // 3. 44 금수 (열린 4 + 닫힌 4 모두 포함)
                if (CountFours(board, x, y, player) >= 2) return true;

                // 4. 33 금수 (무적수/열린 3)
                if (CountOpenThrees(board, x, y, player) >= 2) return true;
            }
            finally
            {
                // 5. 반드시 원상복구
                board[x, y] = PlayerType.None;
            }

            return false;
        }

        private static bool CheckOverline(PlayerType[,] board, int x, int y, PlayerType player)
        {
            for (int i = 0; i < 4; i++)
            {
                if (GetContinuousCount(board, x, y, dx[i], dy[i], player) >= 6) return true;
            }
            return false;
        }

        private static int CountFours(PlayerType[,] board, int x, int y, PlayerType player)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if (IsFour(board, x, y, dx[i], dy[i], player)) count++;
            }
            return count;
        }

        private static int CountOpenThrees(PlayerType[,] board, int x, int y, PlayerType player)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if (IsOpenThree(board, x, y, dx[i], dy[i], player)) count++;
            }
            return count;
        }

        private static bool IsFour(PlayerType[,] board, int x, int y, int dX, int dY, PlayerType player)
        {
            int potentialFives = 0;
            for (int start = -4; start <= 0; start++)
            {
                int stones = 0, empty = 0, emptyX = -1, emptyY = -1;
                for (int k = 0; k < 5; k++)
                {
                    int nx = x + dX * (start + k), ny = y + dY * (start + k);
                    if (!IsInBoard(board, nx, ny)) { stones = -1; break; }
                    if (board[nx, ny] == player) stones++;
                    else if (board[nx, ny] == PlayerType.None) { empty++; emptyX = nx; emptyY = ny; }
                    else { stones = -1; break; }
                }
                if (stones == 4 && empty == 1)
                {
                    board[emptyX, emptyY] = player;
                    bool isFive = GetContinuousCount(board, emptyX, emptyY, dX, dY, player) == 5;
                    board[emptyX, emptyY] = PlayerType.None;
                    if (isFive) potentialFives++;
                }
            }
            return potentialFives > 0;
        }

        private static bool IsOpenThree(PlayerType[,] board, int x, int y, int dX, int dY, PlayerType player)
        {
            for (int start = -3; start <= 0; start++)
            {
                int stones = 0, empty = 0;
                List<(int, int)> blanks = new List<(int, int)>();
                for (int k = 0; k < 4; k++)
                {
                    int nx = x + dX * (start + k), ny = y + dY * (start + k);
                    if (!IsInBoard(board, nx, ny)) { stones = -1; break; }
                    if (board[nx, ny] == player) stones++;
                    else if (board[nx, ny] == PlayerType.None) { empty++; blanks.Add((nx, ny)); }
                    else { stones = -1; break; }
                }

                if (stones == 3 && empty == 1)
                {
                    int bx = blanks[0].Item1, by = blanks[0].Item2;
                    // 가상 돌을 놓기 전, 그 자리가 금수인지 먼저 체크 (33 금수 재귀 방지)
                    if (IsForbidden(board, bx, by, player)) continue;

                    board[bx, by] = player;
                    bool makesOpenFour = IsOpenFour(board, bx, by, dX, dY, player);
                    board[bx, by] = PlayerType.None;

                    if (makesOpenFour) return true;
                }
            }
            return false;
        }

        private static bool IsOpenFour(PlayerType[,] board, int x, int y, int dX, int dY, PlayerType player)
        {
            if (GetContinuousCount(board, x, y, dX, dY, player) != 4) return false;

            int left = CountContinuous(board, x, y, -dX, -dY, player);
            int right = CountContinuous(board, x, y, dX, dY, player);

            int lx = x - dX * (left + 1), ly = y - dY * (left + 1);
            int rx = x + dX * (right + 1), ry = y + dY * (right + 1);

            return IsInBoard(board, lx, ly) && board[lx, ly] == PlayerType.None &&
                   IsInBoard(board, rx, ry) && board[rx, ry] == PlayerType.None;
        }

        private static int GetContinuousCount(PlayerType[,] board, int x, int y, int dX, int dY, PlayerType player)
        {
            return 1 + CountContinuous(board, x, y, dX, dY, player) + CountContinuous(board, x, y, -dX, -dY, player);
        }

        private static int CountContinuous(PlayerType[,] board, int x, int y, int dX, int dY, PlayerType player)
        {
            int count = 0;
            int nx = x + dX, ny = y + dY;
            while (IsInBoard(board, nx, ny) && board[nx, ny] == player)
            {
                count++; nx += dX; ny += dY;
            }
            return count;
        }

        private static bool IsInBoard(PlayerType[,] board, int x, int y)
            => x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1);
    }
}
