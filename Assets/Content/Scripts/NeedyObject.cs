using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[RequireComponent (typeof(CircleCollider2D))]
public class NeedyObject : MonoBehaviour {

    public int fuelNeed;
    public int foodNeed;
    public int waterNeed;
    public int energyNeed;

    protected bool fulfilled;

    Dictionary<string, Collider2D> collidersInTrigger = new Dictionary<string, Collider2D>(); 

    RectTransform needBubble = null;
    RectTransform canvas;
    public Transform anchorPoint;

	// Use this for initialization
	internal virtual void Start () {
        GameObject go = GameObject.Find("Canvas");
        canvas = go.GetComponent<RectTransform>();
	}

    public bool CheckFulfilled() {
        if (fuelNeed == 0 && foodNeed == 0 && waterNeed == 0 && energyNeed == 0) {
            fulfilled = true;
            RemovePopUp();
        }
        else {
            fulfilled = false;
            RemovePopUp();
            MakePopUp();
        }

        return fulfilled;
    }

    public void MakePopUp() {
        GameObject go = Instantiate(Resources.Load("NeedBubble")) as GameObject;
        go.transform.SetParent(canvas.transform);
        needBubble = go.GetComponent<RectTransform>();
        needBubble.localScale = Vector3.one;

        if (foodNeed > 0) {
            GameObject icon = Instantiate(Resources.Load("NeedIcon")) as GameObject;
            icon.transform.SetParent(needBubble.FindChild("NeedList"));
            icon.GetComponent<RectTransform>().localScale = Vector3.one;
            icon.GetComponent<Image>().color = SupplyType.GetSupplyColorByCode(SupplyCode.Food);
            icon.transform.FindChild("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = foodNeed.ToString();
        }
        if (energyNeed > 0) {
            GameObject icon = Instantiate(Resources.Load("NeedIcon")) as GameObject;
            icon.transform.SetParent(needBubble.FindChild("NeedList"));
            icon.GetComponent<RectTransform>().localScale = Vector3.one;
            icon.GetComponent<Image>().color = SupplyType.GetSupplyColorByCode(SupplyCode.Energy);
            icon.transform.FindChild("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = energyNeed.ToString();
        }
        if (waterNeed > 0) {
            GameObject icon = Instantiate(Resources.Load("NeedIcon")) as GameObject;
            icon.transform.SetParent(needBubble.FindChild("NeedList"));
            icon.GetComponent<RectTransform>().localScale = Vector3.one;
            icon.GetComponent<Image>().color = SupplyType.GetSupplyColorByCode(SupplyCode.Water);
            icon.transform.FindChild("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = waterNeed.ToString();
        }
        if (fuelNeed > 0) {
            GameObject icon = Instantiate(Resources.Load("NeedIcon")) as GameObject;
            icon.transform.SetParent(needBubble.FindChild("NeedList"));
            icon.GetComponent<RectTransform>().localScale = Vector3.one;
            icon.GetComponent<Image>().color = SupplyType.GetSupplyColorByCode(SupplyCode.Fuel);
            icon.transform.FindChild("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = fuelNeed.ToString();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        if (!collidersInTrigger.ContainsKey(collider.name)) {
            collidersInTrigger.Add(collider.name, collider);
        }
        else {
            collidersInTrigger.Remove(collider.name);
            collidersInTrigger.Add(collider.name, collider);
        }

        if (collider.name == "Player" && !CheckFulfilled() && needBubble == null) {
            MakePopUp();
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        collidersInTrigger.Remove(collider.name);

        RemovePopUp();
    }

    public void RemovePopUp() {
        if(needBubble != null)
            GameObject.Destroy(needBubble.gameObject);
    }

	// Update is called once per frame
    internal virtual void Update() {
        if (needBubble != null) {
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(anchorPoint.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x)),
            (ViewportPosition.y * canvas.sizeDelta.y));

            //now you can set the position of the ui element
            needBubble.anchoredPosition = WorldObject_ScreenPosition;

        }

        if (Input.GetButtonDown("Interact") && collidersInTrigger.ContainsKey("Player")) {
            Player player = collidersInTrigger["Player"].GetComponent<Player>();
            if (player.carryingItem && player.item.GetComponent<SupplyItem>()) {
                if (player.item.GetComponent<SupplyItem>().supplyCode == SupplyCode.Energy && energyNeed > 0) {
                    energyNeed = Mathf.Clamp(energyNeed - 1, 0, int.MaxValue);
                    TakePlayerItem(player);
                }
                else if (player.item.GetComponent<SupplyItem>().supplyCode == SupplyCode.Fuel && fuelNeed > 0) {
                    fuelNeed = Mathf.Clamp(fuelNeed - 1, 0, int.MaxValue);
                    TakePlayerItem(player);
                }
                else if (player.item.GetComponent<SupplyItem>().supplyCode == SupplyCode.Food && foodNeed > 0) {
                    foodNeed = Mathf.Clamp(foodNeed - 1, 0, int.MaxValue);
                    TakePlayerItem(player);
                }
                else if (player.item.GetComponent<SupplyItem>().supplyCode == SupplyCode.Water && waterNeed > 0) {
                    waterNeed = Mathf.Clamp(waterNeed - 1, 0, int.MaxValue);
                    TakePlayerItem(player);
                }

            }
        }
	}

    void TakePlayerItem(Player player) {
        GameObject.Destroy(player.item.gameObject);
        player.carryingItem = false;
        CheckFulfilled();
    }
}
