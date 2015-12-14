using UnityEngine;
using System.Collections;

public class NeedyNPC : NeedyObject {

    public string rescueMessage;
    public Color fulfilledColor;
    float maxTimeToTransition = 3;
    float timeToTransition = 3;
    bool fulfillmentAnimationsPlayed = false;
    public bool unlockBuilding;

	// Use this for initialization
	internal override void Start () {
        base.Start();
	}

    // Update is called once per frame
    internal override void Update() {
        base.Update();

        if (fulfilled) {
            if (GetComponent<SpriteRenderer>().color != fulfilledColor) {
                timeToTransition -= Time.deltaTime;
                GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, fulfilledColor, 1 - timeToTransition / maxTimeToTransition);
            }
            if (!fulfillmentAnimationsPlayed) {
                LeanTween.rotateZ(gameObject, 0, 0.5f);
                LeanTween.moveY(gameObject, -2.5f, 0.5f);

                if (rescueMessage != "") {
                    GameObject.FindGameObjectWithTag("Player").transform.FindChild("TextAnchor").GetComponent<Teleprompter>().PrepMessage(rescueMessage);
                }
                fulfillmentAnimationsPlayed = true;
                if (unlockBuilding) {
                    GameObject.Find("Level").GetComponent<BuildingOverlord>().BuildNextBuilding();
                }
            }

        }
        else {

        }
    }
}
