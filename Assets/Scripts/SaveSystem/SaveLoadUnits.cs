using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadUnits : MonoBehaviour, IsSaveable
{
    public GameObject[] dataUnits, dataBuildings;
    private Unit[] units;
    private Building[] buildings;
    private AIGeneral aiGeneral;

    bool timer = false;
    bool timerActive = false;

    private void Start()
    {
        GameObject general = GameObject.Find("/AIGeneral");
        if (general != null){
            aiGeneral = GameObject.Find("/AIGeneral").GetComponent<AIGeneral>();
        };
        
    }

    [System.Serializable]
    struct UnitData
    {
        public string[] unitTypes, buildingTypes;
        public float[] unitsHealth, buildingsHealth;
        public string[] unitsID, buildingsID, targetsID;
        public float[,] unitPositions, unitRotations, buildingsPositions;
        public int[] unitTeams, buildingTeams;
        public bool[] isConstructed;

    }

    public object CaptureState()
    {
        UnitData data;
        
        int pos = 0;
        GameObject[] unitsSave = GameObject.FindGameObjectsWithTag("Unit");

        data.unitTypes = new string[unitsSave.Length];
        data.unitsHealth = new float[unitsSave.Length];
        data.unitPositions = new float[unitsSave.Length,3];
        
        data.unitRotations = new float[unitsSave.Length,4];
        data.unitsID = new string[unitsSave.Length];
        data.targetsID = new string[unitsSave.Length];
        data.unitTeams = new int[unitsSave.Length];

        foreach (GameObject unit in unitsSave)
        {
            Unit unitData = unit.GetComponent<Unit>();
            data.unitTypes[pos] = unitData.typeOfUnit;
            data.unitsHealth[pos] = unit.GetComponent<ObjectLife>().getHealth();
            data.unitsID[pos] = unitData.id;
            data.unitTeams[pos] = unitData.team;

            // Posició
            data.unitPositions[pos,0] = unit.transform.position.x;
            data.unitPositions[pos,1] = unit.transform.position.y;
            data.unitPositions[pos,2] = unit.transform.position.z;

            // Rotació
            data.unitRotations[pos,0] = unit.transform.rotation.x;
            data.unitRotations[pos,1] = unit.transform.rotation.y;
            data.unitRotations[pos,2] = unit.transform.rotation.z;
            data.unitRotations[pos,3] = unit.transform.rotation.w;

            if (unitData.target!= null)
            {
                if (unitData.target.gameObject.tag.Equals("Building"))
                {
                    data.targetsID[pos] = unitData.target.gameObject.GetComponentInParent<Building>().id;
                }
                else
                {
                    data.targetsID[pos] = unitData.target.gameObject.GetComponent<Unit>().id;
                }
                
            }
            else
            {
                data.targetsID[pos] = null;
            }
            

            pos += 1;
        }

        

        pos = 0;
        GameObject[] buildingsSave = GameObject.FindGameObjectsWithTag("Building");

        data.buildingTypes = new string[buildingsSave.Length];
        data.buildingsHealth = new float[buildingsSave.Length];
        data.buildingsPositions = new float[buildingsSave.Length,3];
        data.buildingsID = new string[buildingsSave.Length];
        data.buildingTeams = new int[buildingsSave.Length];
        data.isConstructed = new bool[buildingsSave.Length];

        foreach (GameObject building in buildingsSave)
        {
            Building buildingData = building.GetComponentInParent<Building>();
            data.buildingTypes[pos] = buildingData.data.BuildingName;
            data.buildingsHealth[pos] = building.GetComponent<ObjectLife>().getHealth();
            data.buildingsID[pos] = buildingData.id;
            data.buildingTeams[pos] = buildingData.team;
            data.isConstructed[pos] = buildingData.constructed;

            // faig això perque quan instancio el PetrolPump surt mes amunt i en diagonal 
            if (buildingData.data.BuildingName.Equals("PetrolPump"))
            {
                data.buildingsPositions[pos, 0] = building.transform.position.x-1f;
                data.buildingsPositions[pos, 1] = building.transform.position.y-1.5f;
                data.buildingsPositions[pos, 2] = building.transform.position.z-1f;
            }
            else{
                data.buildingsPositions[pos, 0] = building.transform.position.x;
                data.buildingsPositions[pos, 1] = building.transform.position.y;
                data.buildingsPositions[pos, 2] = building.transform.position.z;
            }
            

            pos += 1;
        }

        return data;
    }

    public void RestoreState(object dataLoaded)
    {
        timer = false;
        timerActive = false;
        UnitData data = (UnitData)dataLoaded;

        while (timer)
        {
            if (!timerActive)
            {
                StartCoroutine(timerCoroutina());
            }
        }

        GameObject[] asdf = GameObject.FindGameObjectsWithTag("Building");
        GameObject[] asdfr = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject building in asdf)
        {
            Destroy(building.transform.parent.gameObject);
        }
        foreach (GameObject unit in asdfr)
        {
            Destroy(unit);
        }

        GameObject general = GameObject.Find("/AIGeneral");
        if (general != null)
        {
            aiGeneral = GameObject.Find("/AIGeneral").GetComponent<AIGeneral>();
        };

        units = new Unit[data.unitTypes.Length];
        buildings = new Building[data.buildingTypes.Length];

        int pos = 0;
        foreach (string unitType in data.unitTypes)
        {
            GameObject unit = null;
            ////// --------------  Arreglar quan hi haguin totes les unitats ------------------------ //////////////
            if (unitType.Equals("Tank"))
            {
                unit = Instantiate(dataUnits[0],
                new Vector3(data.unitPositions[pos,0], data.unitPositions[pos,1], data.unitPositions[pos,2]),
                new Quaternion(data.unitRotations[pos,0], data.unitRotations[pos,1], data.unitRotations[pos,2], data.unitRotations[pos,3]));
            }
            else{
                unit = Instantiate(dataUnits[0],
                new Vector3(data.unitPositions[pos, 0], data.unitPositions[pos, 1], data.unitPositions[pos, 2]),
                new Quaternion(data.unitRotations[pos, 0], data.unitRotations[pos, 1], data.unitRotations[pos, 2], data.unitRotations[pos, 3]));
            }

            Unit unitComponent = unit.GetComponentInChildren<Unit>();
            Debug.Log(unitComponent);
            units[pos] = unitComponent;
            unitComponent.team = data.unitTeams[pos];

            units[pos].health = data.unitsHealth[pos];
            units[pos].id = data.unitsID[pos];
            pos += 1;
        }

        // Instancio els edificis;
        int numType = 0;
        pos = 0;
        foreach (string buildingType in data.buildingTypes)
        {
            ////// --------------  Arreglar quan hi haguin tots els edificis ------------------------ //////////////
            GameObject building = null;
            switch(buildingType)
            {
                case "PetrolPump":
                    numType = 0;
                    break;
                case "Mine":
                    numType = 1;
                    break;
                case "EnergyPlant":
                    numType = 2;
                    break;
                case "LogisticCenter":
                    numType = 3;
                    break;
                case "WarFactory":
                    numType = 4;
                    break;


            }
            

            building = Instantiate(dataBuildings[numType],
                new Vector3(data.buildingsPositions[pos,0], data.buildingsPositions[pos,1], data.buildingsPositions[pos,2]),
                new Quaternion(0, 0, 0, 0));
            Building buildingComponent = building.GetComponent<Building>();
            buildings[pos] = buildingComponent;
            buildingComponent.team = data.buildingTeams[pos];
            buildingComponent.constructed = data.isConstructed[pos];
            building.GetComponentInChildren<ObjectLife>().setHealth(data.buildingsHealth[pos]);

            Material[] materials = building.GetComponent<Building>().materials;
            //////////////  Cambiar els colors depenent de l'edifici
            ///
            if (buildingComponent.constructed)
            {
                foreach (Material material in materials)
                {
                    material.color = new Color(1f, 1f, 1f, 1f);
                }
                buildingComponent.adjustMaterials();
                buildingComponent.renderer.material = buildingComponent.opaqueMat;
            }
            else
            {

            }
            
            pos += 1;
        }

        // Quan ja estan instanciades totes les unitats els hi poso els targets i si són de l'equip 2 els hi assigno a la IA enemiga
        pos = 0;
        int posAI = 0;
        foreach(Unit unit in units)
        {
            
            if (unit.target != null)
            {
                // miro les ID de totes les unitats
                foreach (Unit target in units)
                {
                    if (target.id == data.targetsID[pos])
                    {
                        unit.target = target.transform;
                        break;
                    }
                }
                // si el target no es una unitat miro els edificis
                if (unit.target == null)
                {
                    foreach (Building target in buildings)
                    {
                        if (target.id == data.targetsID[pos])
                        {
                            unit.target = target.GetComponentInChildren<Transform>();
                            break;
                        }
                    }
                }
            }
            if (unit.team == 2)
            {
                switch (posAI)
                {
                    case 0:
                        aiGeneral.unit1 = unit;
                        break;
                    case 1:
                        aiGeneral.unit2 = unit;
                        break;
                    case 2:
                        aiGeneral.unit3 = unit;
                        break;
                    case 3:
                        aiGeneral.unit4 = unit;
                        break;
                }
                posAI += 1;
            }
            pos += 1;
        }
    }
    IEnumerator timerCoroutina()
    {
        timerActive = true;
        yield return new WaitForSeconds(5);
        timerActive = false;
        timer = true;
    }
}
