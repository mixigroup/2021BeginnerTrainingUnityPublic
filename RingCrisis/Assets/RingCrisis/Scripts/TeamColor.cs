namespace RingCrisis
{
    /// <summary>
    /// チームカラー
    /// 1 vs 1 のゲームなのでチームではないが…
    /// </summary>
    public enum TeamColor
    {
        Red,
        Blue
    }

    /// <summary>
    /// 列挙型TeamColorの拡張メソッド
    /// 列挙型には直接メソッドを定義できないが、拡張メソッドを使うことでインスタンスメソッドのように呼び出すことができる
    /// もちろん拡張メソッドは列挙型以外の通常のクラス・構造体に対しても定義可能
    /// </summary>
    public static class TeamColorExtension
    {
        /// <summary>
        /// 相手のチームカラーを返します
        /// </summary>
        public static TeamColor Opponent(this ref TeamColor teamColor)
        {
            return teamColor == TeamColor.Red ? TeamColor.Blue : TeamColor.Red;
        }
    }
}
