using UnityEngine;
using System.Collections;

public class NeedyGenerator : NeedyObject {

    public PoweredItem[] poweredItems;
    GameObject spawnedSupply = null;

	// Use this for initialization
    internal override void Start() {
        foreach (PoweredItem go in poweredItems) {
            LineRenderer lr = go.GetComponent<LineRenderer>();
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, go.transform.position);
            //lr.enabled = false;
        }
        base.Start();
	}

    void GeneratorAnimation() {
        float height = Mathf.PerlinNoise(9.5f, 0f) * 10f;

        float shakeAmt = height * 0.2f; // the degrees to shake the camera
        float shakePeriodTime = 0.42f; // The period of each shake
        LeanTween.rotateAroundLocal(gameObject, Vector3.forward, shakeAmt, shakePeriodTime)
        .setEase(LeanTweenType.easeShake) // this is a special ease that is good for shaking
        .setLoopClamp()
        .setRepeat(-1);
    }

	// Update is called once per frame
    internal override void Update() {
        base.Update();

        if (fulfilled && !LeanTween.isTweening(gameObject)) {
            if (poweredItems.Length > 0) {
                foreach (PoweredItem go in poweredItems) {
                    go.PowerUp();
                }
            }
            GeneratorAnimation();
        } else if (fulfilled && spawnedSupply == null) {
            spawnedSupply = Instantiate(Resources.Load("Supply"), transform.FindChild("SpawnLocation").position, Quaternion.identity) as GameObject;
            spawnedSupply.GetComponent<SpriteRenderer>().color = SupplyType.GetSupplyColorByCode(SupplyCode.Energy);
            spawnedSupply.GetComponent<SupplyItem>().supplyCode = SupplyCode.Energy;
        }
	}
}
