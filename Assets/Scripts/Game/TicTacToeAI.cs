using UnityEngine;
using static Omok.Constants;

namespace Omok {

    public static class TicTacToeAI {

        public static (int row, int col)? GetBestMove(PlayerType[,] board){
            float bestScore = float.MinValue;
            (int row, int col) bestMove = (-1, -1);

            for (int row = 0; row < board.GetLength(0); row++){
                for (int col = 0; col < board.GetLength(1); col++){
                    if (board[row, col] == PlayerType.None){
                        board[row, col] = PlayerType.Player2; // AI의 턴
                        float score = DoMinimax(board, 0, false);
                        board[row, col] = PlayerType.None; // 되돌리기

                        if (score > bestScore){
                            bestScore = score;
                            bestMove = (row, col);
                        }
                    }
                }
            }

            if (bestMove != (-1, -1)){
                return bestMove;
            }

            return null;
        }

        private static float DoMinimax(PlayerType[,] board, int depth, bool isMaximizing){
            // 게임 결과 확인
            if (CheckGameWin(PlayerType.Player1, board)) return -10 + depth;
            if (CheckGameWin(PlayerType.Player2, board)) return 10 - depth;
            if (CheckGameDraw(board)) return 0;

            if (isMaximizing){
                float bestScore = float.MinValue;
                for (int row = 0; row < board.GetLength(0); row++){
                    for (int col = 0; col < board.GetLength(1); col++){
                        if (board[row, col] == PlayerType.None){
                            board[row, col] = PlayerType.Player2; // AI의 턴
                            float score = DoMinimax(board, depth + 1, false);
                            board[row, col] = PlayerType.None; // 되돌리기
                            bestScore = Mathf.Max(score, bestScore);
                        }
                    }
                }

                return bestScore;
            }
            else{
                float bestScore = float.MaxValue;
                for (int row = 0; row < board.GetLength(0); row++){
                    for (int col = 0; col < board.GetLength(1); col++){
                        if (board[row, col] == PlayerType.None){
                            board[row, col] = PlayerType.Player1; // 플레이어의 턴
                            float score = DoMinimax(board, depth + 1, true);
                            board[row, col] = PlayerType.None; // 되돌리기
                            bestScore = Mathf.Min(score, bestScore);
                        }
                    }
                }

                return bestScore;
            }
        }

        public static bool CheckGameWin(PlayerType playerType, PlayerType[,] board){
            // 가로 체크
            for (int row = 0; row < BOARD_SIZE; row++){
                for (int col = 0; col <= BOARD_SIZE - 5; col++){
                    if (board[row, col] == playerType &&
                        board[row, col + 1] == playerType &&
                        board[row, col + 2] == playerType &&
                        board[row, col + 3] == playerType &&
                        board[row, col + 4] == playerType)
                        return true;
                }
            }

            // 세로 체크
            for (int col = 0; col < BOARD_SIZE; col++){
                for (int row = 0; row <= BOARD_SIZE - 5; row++){
                    if (board[row, col] == playerType &&
                        board[row + 1, col] == playerType &&
                        board[row + 2, col] == playerType &&
                        board[row + 3, col] == playerType &&
                        board[row + 4, col] == playerType)
                        return true;
                }
            }

            // 대각선 체크 (↘)
            for (int row = 0; row <= BOARD_SIZE - 5; row++){
                for (int col = 0; col <= BOARD_SIZE - 5; col++){
                    if (board[row, col] == playerType &&
                        board[row + 1, col + 1] == playerType &&
                        board[row + 2, col + 2] == playerType &&
                        board[row + 3, col + 3] == playerType &&
                        board[row + 4, col + 4] == playerType)
                        return true;
                }
            }

            // 대각선 체크 (↙)
            for (int row = 0; row <= BOARD_SIZE - 5; row++){
                for (int col = 4; col < BOARD_SIZE; col++){
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

        public static bool CheckGameDraw(PlayerType[,] board){
            for (int row = 0; row < BOARD_SIZE; row++){
                for (int col = 0; col < BOARD_SIZE; col++){
                    if (board[row, col] == PlayerType.None)
                        return false;
                }
            }

            return true;
        }

    }

}