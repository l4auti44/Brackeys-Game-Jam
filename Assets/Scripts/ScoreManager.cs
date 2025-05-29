public static class ScoreManager
{
    private static int score = 0;

    public static void AddScore(int points)
    {
        score += points;
    }

    public static void SubtractScore(int points)
    {
        score -= points;
    }

    public static int GetScore()
    {
        return score;
    }
}

