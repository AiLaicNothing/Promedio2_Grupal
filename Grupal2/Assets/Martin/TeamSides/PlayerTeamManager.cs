using UnityEngine;

public class PlayerTeamManager : MonoBehaviour
{
    public static PlayerTeamManager Instance;
    private bool nextIsRed = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Teams GetNextTeam()
    {
        Teams team;

        if (nextIsRed)
        {
            team = Teams.Red;
        }
        else
        {
            team = Teams.Blue;
        }

        nextIsRed = !nextIsRed;

        return team;
    }
}
