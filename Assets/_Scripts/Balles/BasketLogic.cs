using UnityEngine;

public class BasketLogic : MonoBehaviour
{
    public GameEvent BasketFullEvent;
    public GameEvent BallInBasket;
    public GameEvent BallOutBasket;
    public GameEvent OtherInBasket;
    public int BallsNeeded = 3;
    private int currentBalls = 0;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if it's a ball
        Balle ball = other.GetComponent<Balle>();

        if (ball == null )
        {
            OtherInBasket.TriggerEvent();
        }

        // 2. Only proceed if it IS a ball AND it hasn't been counted yet
        else if (!ball.isAlreadyInBasket)
        {
            ball.isAlreadyInBasket = true; // Mark it so it can't be counted again
            currentBalls++;

            Debug.Log("Unique ball added! Count: " + currentBalls);

            if (currentBalls >= BallsNeeded)
            {
                Debug.Log("Door Open");
                BasketFullEvent.TriggerEvent();
            }
            else
            {
                BallInBasket.TriggerEvent();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 1. Check if it's a ball
        Balle ball = other.GetComponent<Balle>();

        // 2. Only proceed if it IS a ball AND it hasn't been counted yet
        if (ball != null && ball.isAlreadyInBasket)
        {
            BallOutBasket.TriggerEvent();
            ball.isAlreadyInBasket = false;
            currentBalls--;

            Debug.Log("Unique ball removed! Count: " + currentBalls);
        }
    }
}