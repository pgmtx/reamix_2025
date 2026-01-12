using UnityEngine;

public class BasketLogic : MonoBehaviour
{
    public GameEvent basketFullEvent;
    public GameEvent ballIsInBasket;
    public int ballsNeeded = 3;
    private int currentBalls = 0;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if it's a ball
        Balle ball = other.GetComponent<Balle>();

        // 2. Only proceed if it IS a ball AND it hasn't been counted yet
        if (ball != null && !ball.isAlreadyInBasket)
        {
            ballIsInBasket.TriggerEvent();
            ball.isAlreadyInBasket = true; // Mark it so it can't be counted again
            currentBalls++;

            Debug.Log("Unique ball added! Count: " + currentBalls);

            if (currentBalls >= ballsNeeded)
            {
                Debug.Log("Door Open");
                basketFullEvent.TriggerEvent();
            }
        }
    }
}