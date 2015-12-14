using UnityEngine;
using System.Collections;

public class PoweredDoor : PoweredItem {

    public override void PowerUp() {
        gameObject.SetActive(false);
        gameObject.GetComponent<LineRenderer>().enabled = false;
    }

    public override void PowerDown() {
        gameObject.SetActive(true);
        gameObject.GetComponent<LineRenderer>().enabled = true;
    }
}
