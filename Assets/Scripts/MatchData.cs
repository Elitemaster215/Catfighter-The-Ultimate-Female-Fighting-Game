public static class MatchData
{
    public static int Fighter1Wins = 0;
    public static int Fighter2Wins = 0;
    public static int RoundsToWin = 2; // First to 2 wins (Best of 3)

    public static void ResetMatch()
    {
        Fighter1Wins = 0;
        Fighter2Wins = 0;
    }

    public static bool HasFighter1WonMatch() => Fighter1Wins >= RoundsToWin;
    public static bool HasFighter2WonMatch() => Fighter2Wins >= RoundsToWin;
}