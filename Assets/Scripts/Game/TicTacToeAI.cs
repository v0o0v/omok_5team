using UnityEngine;
using static Tictactoe.Constants;

namespace Tictactoe {

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
            for (int row = 0; row < BOARD_SIZE; row++){
                if (board[row, 0] == playerType &&
                    board[row, 1] == playerType &&
                    board[row, 2] == playerType)
                    return true;
            }

            for (int col = 0; col < BOARD_SIZE; col++){
                if (board[0, col] == playerType &&
                    board[1, col] == playerType &&
                    board[2, col] == playerType)
                    return true;
            }

            if (board[0, 0] == playerType &&
                board[1, 1] == playerType &&
                board[2, 2] == playerType)
                return true;
            if (board[0, 2] == playerType &&
                board[1, 1] == playerType &&
                board[2, 0] == playerType)
                return true;

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