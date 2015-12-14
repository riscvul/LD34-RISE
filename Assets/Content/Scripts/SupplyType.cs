using UnityEngine;
using System.Collections;

public enum SupplyCode {
    Energy,
    Food,
    Water,
    Fuel
}

public class SupplyType {
    public static Color[] supplyColors = {Color.yellow, Color.green, Color.cyan, Color.red};

    public static Color GetSupplyColorByCode(SupplyCode supplyCode) {
        return supplyColors[(int)supplyCode];
    }
}

