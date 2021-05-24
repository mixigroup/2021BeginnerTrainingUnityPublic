namespace RingCrisis
{
    public class GameData
    {
        public TeamColor TeamColor { get; private set; }

        public int Score { get; set; }

        public GameData(TeamColor teamColor)
        {
            TeamColor = teamColor;
        }
    }
}
