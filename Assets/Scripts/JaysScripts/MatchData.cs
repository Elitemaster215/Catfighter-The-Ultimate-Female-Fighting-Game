
public static class MatchData
{
    //public static int PlayerWins = 0;
    //public static int AIWins = 0;
    public static int Fighter1Wins = 0;
    public static int Fighter2Wins = 0;
    public static int RoundsToWin = 3; // First to 2 wins (Best of 3)

    public static bool fightOngoing = false;
    public static bool matchisPvAI = true;

    public static void ResetMatch()
    {
        //PlayerWins = 0;
        //AIWins = 0;

        Fighter1Wins = 0;
        Fighter2Wins = 0;

        fightOngoing = false;
    }

    public static void MatchOngoing()
    {
        fightOngoing = true;
    }
    //public static bool HasPlayerWonMatch() => PlayerWins >= RoundsToWin;
    //public static bool HasAIWonMatch() => AIWins >= RoundsToWin;

    public static bool HasFighter1WonMatch() => Fighter1Wins >= RoundsToWin;
    public static bool HasFighter2WonMatch() => Fighter2Wins >= RoundsToWin;
}