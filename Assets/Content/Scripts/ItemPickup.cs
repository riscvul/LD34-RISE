using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class ItemPickup : MonoBehaviour {
	
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Player>().carryingItem == false) {
            collision.gameObject.GetComponent<Player>().GiveItem(gameObject);
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

}
