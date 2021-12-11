using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    private Transform player;
    private Vector3 startOffset;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startOffset = transform.position - player.position;
    }

    private void Update() {
        transform.position = player.position + startOffset;
    }
}
