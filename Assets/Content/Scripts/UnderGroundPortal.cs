using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnderGroundPortal : MonoBehaviour {

    Dictionary<string, Collider2D> collidersInTrigger = new Dictionary<string, Collider2D>();
    public GameObject undergroundCover;
    public bool aboveGround;

    public Vector3 oppositePosition;

    void OnTriggerEnter2D(Collider2D collider) {
        if (!collidersInTrigger.ContainsKey(collider.name)) {
            collidersInTrigger.Add(collider.name, collider);
        }
        else {
            collidersInTrigger.Remove(collider.name);
            collidersInTrigger.Add(collider.name, collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        collidersInTrigger.Remove(collider.name);
    }

    void Update() {
        if (Input.GetButtonDown("Interact") && collidersInTrigger.ContainsKey("Player")) {
            collidersInTrigger["Player"].transform.position = oppositePosition;
            bool coverActive = aboveGround ? false : true;
            undergroundCover.SetActive(coverActive);
            if (!aboveGround) {
                Camera.main.transform.FindChild("Snow").GetComponent<ParticleSystem>().Play();
            }
            else {
                Camera.main.transform.FindChild("Snow").GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}
