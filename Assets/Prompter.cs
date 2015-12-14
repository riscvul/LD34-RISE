﻿using UnityEngine;
using System.Collections;

public class Prompter : MonoBehaviour {

    public string message;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "Player") {
            collider.transform.FindChild("TextAnchor").GetComponent<Teleprompter>().PrepMessage(message);
            GameObject.Destroy(gameObject);
        }
    }
}
