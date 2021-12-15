using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {
    public static EventManager Instance;

    public Vector3 checkPosition;
    
    //Lista di UnityActions... (deathEvent, darknessEvent)

    public UnityAction darknessEvent;
    public UnityAction darknessBanishedEvent;
    public UnityAction darkEnteredEvent;
    public UnityAction deathEvent;
    public UnityAction getUpEvent;

    private void Awake() {
        Instance = this;
    }
    
    //Lista di Funzioni alle quali sottoscriversi... (OnDeath, OnDarkness)

    public void OnDarkness() {
        darknessEvent?.Invoke();
    }

    public void OnDarknessBanished() {
        darknessBanishedEvent?.Invoke();
    }

    public void OnDarkEntered() {
        darkEnteredEvent?.Invoke();
    }

    public void OnDeath() {
        deathEvent?.Invoke();
    }

    public void OnGettingUp() {
        getUpEvent?.Invoke();
    }
}
