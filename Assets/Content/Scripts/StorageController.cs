using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (BoxCollider2D))]
public class StorageController : MonoBehaviour {

    Dictionary<string, Collider2D> collidersInTrigger = new Dictionary<string,Collider2D>(); 

    public SupplyCode supplyCode;
    public float maxSupplyAmount;
    public float curSupplyAmount;

    float curFillPercent;
    SpriteRenderer supplyFillIcon;
    Vector3 minFillSize = new Vector3(0.1f, 0.1f, 1f);
    Vector3 maxFillPosition = new Vector3(-0.002f, 0.5f, 0);
    Vector3 minFillPosition = new Vector3(-0.002f, 0.06f, 0);

	// Use this for initialization
	void Start () {
        supplyFillIcon = transform.FindChild("Fill").GetComponent<SpriteRenderer>();
        supplyFillIcon.color = SupplyType.GetSupplyColorByCode(supplyCode);
        AdjustInventory(0);
	}

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

    public bool TakeSupplyFromStorage() {
        return AdjustInventory(-1);
    }

    public bool AddSupplyToStorage() {
        return AdjustInventory(1);
    }

    bool AdjustInventory(float change) {
        if (curSupplyAmount + change < 0 || curSupplyAmount + change > maxSupplyAmount) {
            return false;
        }
        else {
            curSupplyAmount = Mathf.Clamp(curSupplyAmount + change, 0, maxSupplyAmount);
            curFillPercent = curSupplyAmount / maxSupplyAmount;
            if (curFillPercent > 0) {
                supplyFillIcon.transform.localScale = Vector3.Lerp(Vector3.one, minFillSize, 1 - curFillPercent);
                supplyFillIcon.transform.localPosition = Vector3.Lerp(maxFillPosition, minFillPosition, 1 - curFillPercent);
            }
            else {
                supplyFillIcon.transform.localScale = Vector3.zero;
            }
            return true;
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Interact") && collidersInTrigger.ContainsKey("Player")) {
            Player player = collidersInTrigger["Player"].GetComponent<Player>();
            if (player.carryingItem && player.item.GetComponent<SupplyItem>()) {
                if(curSupplyAmount <= 0 || player.item.GetComponent<SupplyItem>().supplyCode == supplyCode) {
                    if (curSupplyAmount <= 0) {
                        supplyCode = player.item.GetComponent<SupplyItem>().supplyCode;
                        transform.FindChild("Fill").GetComponent<SpriteRenderer>().color = SupplyType.GetSupplyColorByCode(player.item.GetComponent<SupplyItem>().supplyCode);
                    }
                    AddSupplyToStorage();
                    GameObject.Destroy(player.item.gameObject);
                    player.carryingItem = false;
                }
            }
            else {
                TakeSupplyFromStorage();
                GameObject go = Instantiate(Resources.Load("Supply")) as GameObject;
                Debug.Log(go);
                go.GetComponent<SpriteRenderer>().color = SupplyType.GetSupplyColorByCode(supplyCode);
                go.GetComponent<SupplyItem>().supplyCode = supplyCode;
                go.GetComponent<Rigidbody2D>().isKinematic = true;
                player.GiveItem(go);
            }
        }
	}
}
