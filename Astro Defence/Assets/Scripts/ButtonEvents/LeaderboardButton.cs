using UnityEngine;

public class LeaderboardButton : MonoBehaviour
{
    public void OnClick()
    {
        GUI_Manager.SharedInstance.DisplayMenu("Leaderboard");
    }
}
