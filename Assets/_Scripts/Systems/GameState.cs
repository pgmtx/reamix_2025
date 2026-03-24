using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private bool basketGameFinished;
    private bool slutMachineFinished;

    public event Action<bool> OnSlutMachineFinished;
    public event Action<bool> OnBasketGameFinished;
    public GameEvent BothGamesSalle1Finished;
    
    public bool BasketGameFinished
    {
        get => basketGameFinished;
        set
        {
            if (basketGameFinished == value) return;

            basketGameFinished = value;
            OnBasketGameFinished?.Invoke(basketGameFinished);
        }
    }

    public bool SlutMachineFinished
    {
        get => slutMachineFinished;
        set
        {
            if (slutMachineFinished == value) return;

            slutMachineFinished = value;
            OnSlutMachineFinished?.Invoke(slutMachineFinished);
        }
    }

    private void Awake()
    {
        OnBasketGameFinished += OpenDoorIfBothGamesFinished;
        OnSlutMachineFinished += OpenDoorIfBothGamesFinished;
    }

    public void OpenDoorIfBothGamesFinished(bool newValue)
    {
        if (basketGameFinished && slutMachineFinished)
        {
            BothGamesSalle1Finished.TriggerEvent();
        }
    }
}
