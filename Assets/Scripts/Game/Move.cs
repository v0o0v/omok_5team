using static Omok.Constants;

namespace Omok
{

    public struct Move
    {

        public int X;
        public int Y;
        public PlayerType playerType;

        public Move(int x, int y, PlayerType playerType)
        {
            X = x;
            Y = y;
            this.playerType = playerType;
        }

    }

}