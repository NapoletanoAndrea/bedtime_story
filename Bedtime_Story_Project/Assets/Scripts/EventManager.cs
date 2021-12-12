using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {
    public static EventManager Instance;
    
    //Lista di UnityActions... (deathEvent, darknessEvent)

    public UnityAction darknessEvent;
    public UnityAction darknessBanishedEvent;

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
}
