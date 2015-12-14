using UnityEngine;
using System.Collections;

public class SpawnFirstGirl : MonoBehaviour {

    public Transform girl;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "Player") {
            LeanTween.moveLocal(girl.gameObject, girl.position + new Vector3(-2.25f, 0.85f, 0), 2).setOnComplete(OnComplete);
            //collider.transform.FindChild("TextAnchor").GetComponent<Teleprompter>().PrepMessage(message);
            //GameObject.Destroy(gameObject);
        }
    }

    void OnComplete() {
        LeanTween.rotateZ(girl.gameObject, 70, 0.5f);
        LeanTween.moveY(girl.gameObject, -3.25f, 0.5f).setOnComplete(FinishAndDestroy);
        //girl.GetComponent<ParticleSystem>().Play();
    }

    void FinishAndDestroy() {
        girl.FindChild("SnowFall").GetComponent<ParticleSystem>().Play();
        GameObject.Destroy(this);
    }
}
