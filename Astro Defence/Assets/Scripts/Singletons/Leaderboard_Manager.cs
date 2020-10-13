using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard_Manager : MonoBehaviour
{
    public static Leaderboard_Manager SharedInstance;

    [Tooltip("The scriptable object we will use for the leaderboard (this is where the scores are held")]
    public Leaderboard leaderboard;
    public Text leaderboardText;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void OnEnable()
    {
        SetLeaderboard();
    }

    public void AddToLeaderboard(int score)
    {
        leaderboard.AddScore(score);
    }

    private void SetLeaderboard()
    {
        leaderboardText.text = GetLeaderboard();
    }

    public string GetLeaderboard()
    {
        string text = "";
        List<int> scores = leaderboard.GetScores;

        if (scores.Count == 0)
            return "NO ENTRIES";

        for(int i = 0; i < scores.Count; i++)
        {
            text = text + scores[i].ToString().PadLeft(6, '0') + "\n";
        }

        return text;
    }
}
