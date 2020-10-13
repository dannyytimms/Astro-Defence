using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Leaderboard", menuName = "ScriptableObjects/Leaderboard", order = 1)]
public class Leaderboard : ScriptableObject
{
    public int maxCount = 10;
    private List<int> scores = new List<int>();

    public List<int> GetScores { get { return scores.OrderByDescending(i => i).ToList(); } }

    public void AddScore(int score)
    {
        if(scores.Count < maxCount)
        {
            scores.Add(score);
        }
        else
        {
            scores = scores.OrderByDescending(i => i).ToList(); // make sure we get the lowest number
            scores[maxCount - 1] = score; //replace it. We don't need to sort the list again as it is sorted when it's retrieved.
        }
    }
}
