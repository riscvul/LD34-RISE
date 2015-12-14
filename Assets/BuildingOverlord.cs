using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingOverlord : MonoBehaviour {

    public GameObject[] buildingsToBeBuilt;
    int currentIndex = 0;

    public void BuildNextBuilding() {
        Debug.Log("here");
        buildingsToBeBuilt[currentIndex].SetActive(true);
        LeanTween.scale(buildingsToBeBuilt[currentIndex], Vector3.one, 1);
        currentIndex++;
    }
}
