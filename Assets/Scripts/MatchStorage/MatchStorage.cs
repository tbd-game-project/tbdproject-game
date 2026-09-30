using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MatchData
{
    public int Num;
    public Vector2Int Coordinate;
    public Vector2Int Direction;
    public Player Owner;
}

public class MatchStorage : MonoBehaviour
{
    public static MatchStorage Instance { get; private set; }
    public List<MatchData> MatchList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }        
        Instance = this;

        MatchList = new List<MatchData>();
        MatchList.Clear();
    }

    public void AddMatchData(int matchNum, Vector2Int matchCoordinate, Vector2Int matchDirection, Player owner)
    {
        MatchData data = new MatchData
        {
            Num = matchNum,
            Coordinate = matchCoordinate,
            Direction = matchDirection,
            Owner = owner
        };
        MatchList.Add(data);
    }

    public void ClearMatchData()
    {
        MatchList.Clear();
    }

    public MatchData PopMatchData()
    {
        if (MatchList.Count == 0)
        {
            Debug.LogWarning("MatchStorage: MatchList is empty.");
            return default;
        }
        MatchData data = MatchList[MatchList.Count - 1];
        MatchList.RemoveAt(MatchList.Count - 1);
        return data;
    }

    public void SortMatchData()
    {
        // Num‚ª¬‚³‚¢‡‚Éƒ\[ƒg
        MatchList.Sort((a, b) => a.Num.CompareTo(b.Num));
    }

    public int GetMatchCount()
    {
        return MatchList.Count;
    }

}
