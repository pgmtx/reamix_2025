using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDistributor : MonoBehaviour
{
    public List<GameObject> InactiveBalls;
    public List<GameObject> ActiveBalls;

    private void Awake()
    {
        InactiveBalls = new List<GameObject>();
        ActiveBalls = new List<GameObject>();

        Transform ballsParent = transform.GetChild(0);

        Transform[] balls = ballsParent.GetComponentsInChildren<Transform>(true);

        foreach(Transform ball in balls)
        {
            if (ball == ballsParent) continue;

            if (ball.gameObject.activeSelf)
            {
                ActiveBalls.Add(ball.gameObject);
            }
            else
            {
                InactiveBalls.Add(ball.gameObject);
            }
        }
    }

    public void DistributeBall()
    {
        int randomIndex = Random.Range(0, InactiveBalls.Count);

        GameObject ball = InactiveBalls[randomIndex];
        InactiveBalls.RemoveAt(randomIndex);
        ball.SetActive(true);
        ActiveBalls.Add(ball);
    }
}
