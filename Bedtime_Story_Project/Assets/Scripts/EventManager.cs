using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public static EventManager Instance;
    
    //Lista di UnityActions... (deathEvent, darknessEvent)

    private void Awake() {
        Instance = this;
    }
    
    //Lista di Funzioni alle quali sottoscriversi... (OnDeath, OnDarkness)
}
