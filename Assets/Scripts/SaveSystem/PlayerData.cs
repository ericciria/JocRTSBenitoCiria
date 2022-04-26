using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerData
{

    public BuildingData[] dataUnits;
    public float[][] positionUnits, targets;
    public float[] cameraPosition = new float[3];
    public int monedes, fusta;


    public PlayerData(CameraController player, BuildingData[] data)
    {
        dataUnits = player.dataUnits;
        positionUnits = player.positionUnits;
        targets = player.targets;
        monedes = player.monedes;
        fusta = player.fusta;
    }

}
