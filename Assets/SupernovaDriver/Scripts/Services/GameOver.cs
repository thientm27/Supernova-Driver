using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class GameOver : ElympicsMonoBehaviour, IInitializable
{
    public void Initialize()
    {

    }
    public void EndGame(float score)
    {
        if (Elympics.IsServer)
        {
            Elympics.EndGame(new ResultMatchPlayerDatas(new List<ResultMatchPlayerData> { new ResultMatchPlayerData { MatchmakerData = new float[1] { score } } }));
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            EndGame(30);
        }
    }
}
 