using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UniqueIdentifier))]
[RequireComponent(typeof(ObjectLife))]

public class Unit : MonoBehaviour, IsSaveable
{

    public UnitData unitData;

    public string unitName;
    public string description;
    public Sprite icon;
    //[SerializeField] private int moneyCost;
    //[SerializeField] private int metalCost;
    public int attackDamage;
    public string typeOfUnit;
    public string damageMultiplierType;
    public int damageMultiplierAmount;
    private int maxHealth;
    private float movementSpeed;
    public float attackDistance;
    public float attackCooldown;
    public float timeSinceLastAttack = Mathf.Infinity;
    public GameObject nearestEnemy;

    public float health = 0;
    public ObjectLife objectLife;
    public int team;
    public Color teamColor;

    public NavMeshAgent agent;
    public Transform target;
    private bool checking;
    public bool constructor;

    Vector3 previousCorner;
    Vector3 currentCorner;

    public MeshRenderer renderer;
    public Material[] materials;

    public UnitStates currentState = new UnitIdleState();
    public string id;
    public Animator anim;



    private void Awake()
    {
        objectLife = GetComponent<ObjectLife>();
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponentInChildren<MeshRenderer>();
        

        unitName = unitData.UnitName;
        description = unitData.Description;
        icon = unitData.Icon;
        attackDamage = unitData.AttackDamage;
        typeOfUnit = unitData.name;
        damageMultiplierType = unitData.DamageMultiplierType;
        damageMultiplierAmount = unitData.DamageMultiplierAmount;
        maxHealth = unitData.MaxHealth;
        movementSpeed = unitData.MovementSpeed;
        attackDistance = unitData.AttackDistance;
        attackCooldown = unitData.AttackCooldown;
        constructor = unitData.Constructor;
        anim = GetComponentInChildren<Animator>();

        materials = renderer.materials;

        id = GetComponent<UniqueIdentifier>().id;

    }
    void Start()
    {
        
        if (team == 1)
        {
            teamColor = new Color(0.1F, 0.1F, 0.7F, 1F);
        }
        else if (team == 2)
        {
            teamColor = new Color(0.7F, 0.1F, 0.1F, 1F);
        }
        changeColor(teamColor);

        if (health != 0)
        {
            objectLife.setHealth(health);
        }

        agent.speed = unitData.MovementSpeed;
        agent.acceleration = agent.speed - 2;
        if (typeOfUnit.Equals("VehicleConstructor"))
        {
            agent.acceleration = agent.acceleration + 10;
        }

        objectLife.setMaxHealth(maxHealth);
        objectLife.setHealth(maxHealth);

        UnitStates nextState = currentState.OnUpdate(this);

    }

    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        UnitStates nextState = currentState.OnUpdate(this);
        if (nextState != null)
        {
            currentState.OnExitState(this);
            currentState = nextState;
            currentState.OnEnterState(this);
        }
        if (!checking && currentState.ToString().Equals("UnitIdleState"))
        {
            checking = true;
            StartCoroutine(getClosestEnemyInRadius());
        }      
    }

    public void setObjective(RaycastHit hit, float offset)
    {
        if (!constructor){
            if (hit.collider.gameObject.tag.Equals("Unit"))
            {
                if (hit.collider.gameObject.GetComponent<Unit>().team != team)
                {
                    target = hit.collider.gameObject.transform;
                }
            }
            else if (hit.collider.gameObject.tag.Equals("Building")) 
            {
                if (hit.collider.gameObject.GetComponentInParent<Building>().team != team) {
                    target = hit.collider.gameObject.transform;

                }
            }
            else
            {
                target = null;
                agent.SetDestination(hit.point + new Vector3(offset, 0, 0));
            }
        }
        else if(constructor && hit.collider.gameObject.tag.Equals("Building"))
        {
            if( hit.collider.gameObject.GetComponentInParent<Building>().team == team)
            {
                target = hit.collider.gameObject.transform;
            } 
        }
        else
        {
            target = null;
            agent.SetDestination(hit.point + new Vector3(offset, 0, 0));
        }
    }
    public void setObjective(RaycastHit hit, Vector3 offset)
    {
        if (!constructor)
        {
            if (hit.collider.gameObject.tag.Equals("Unit"))
            {
                if (hit.collider.gameObject.GetComponent<Unit>().team != team)
                {
                    target = hit.collider.gameObject.transform;
                }
            }
            else if (hit.collider.gameObject.tag.Equals("Building"))
            {
                if (hit.collider.gameObject.GetComponentInParent<Building>().team != team)
                {
                    target = hit.collider.gameObject.transform;

                }
            }
            else
            {
                target = null;
                agent.SetDestination(hit.point + offset);
            }
        }
        else if (constructor && hit.collider.gameObject.tag.Equals("Building"))
        {
            if (hit.collider.gameObject.GetComponentInParent<Building>().team == team)
            {
                target = hit.collider.gameObject.transform;
            }
        }
        else
        {
            target = null;
            agent.SetDestination(hit.point + offset);
        }
    }

    void OnDrawGizmos()
    {
        if (currentState.ToString().Equals("UnitIdleState"))
        {
            Gizmos.color = Color.cyan;
            //Debug.Log("AAAAAAAAAAAAAAA");
        }
        else if (currentState.ToString().Equals("UnitPursueState"))
        {
            Gizmos.color = Color.yellow;
        }
        else if (currentState.ToString().Equals("UnitConstructState"))
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        if (this.transform.Find("Selection").gameObject.activeInHierarchy)
        {
            //Debug.Log("Estic activat");
            //Gizmos.color = Color.red;
            //Debug.Log(currentState);

            DrawWireArc(this.transform.position, this.transform.forward, 10, attackDistance); // width,length
        }
        if (constructor)
        {
            DrawCircle(this.transform.position, this.transform.forward, 2, 20);
        }
        else
        {
            DrawCircle(this.transform.position, this.transform.forward, attackDistance, 20);
        }
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        DrawPath(agent.path);

    }

    public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
    {
        var srcAngles = GetAnglesFromDir(position, dir);
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = anglesRange / maxSteps;
        var angle = srcAngles - anglesRange / 2;
        for (var i = 0; i <= maxSteps; i++)
        {
            var rad = Mathf.Deg2Rad * angle;
            var posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));

            Gizmos.DrawLine(posA, posB);

            angle += stepAngles;
            posA = posB;
        }
        Gizmos.DrawLine(posA, initialPos);
    }

    public static void DrawCircle(Vector3 position, Vector3 dir, float radius, float maxSteps = 20)
    {
        var srcAngles = GetAnglesFromDir(position, dir);
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = 360 / maxSteps;
        var angle = srcAngles - 360 / 2;

        var rad = Mathf.Deg2Rad * angle;
        var posB = initialPos;
        posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
        angle += stepAngles;
        posA = posB;

        for (var i = 1; i <= maxSteps; i++)
        {
            rad = Mathf.Deg2Rad * angle;
            posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));

            Gizmos.DrawLine(posA, posB);

            angle += stepAngles;
            posA = posB;
        }
    }

    static float GetAnglesFromDir(Vector3 position, Vector3 dir)
    {
        var forwardLimitPos = position + dir;
        var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);

        return srcAngles;
    }
    void DrawPath(NavMeshPath path)
    {
        //yield return new WaitForEndOfFrame();
        path = agent.path;
        if (path.corners.Length < 2)
        {
            return;
        }

        previousCorner = path.corners[0];

        int i = 1;
        while (i < path.corners.Length)
        {
            currentCorner = path.corners[i];
            Gizmos.DrawLine(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }

    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Unit");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    IEnumerator checkClosestEnemy()
    {
        yield return new WaitForSeconds(2);
        nearestEnemy = FindClosestEnemy();
        checking = false;
    }

    IEnumerator getClosestEnemyInRadius()
    {
        yield return new WaitForSeconds(2);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackDistance);
        Collider closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (Collider co in hitColliders)
        {
            if (co.gameObject.tag.Equals("Unit"))
            {
                if (co.gameObject.GetComponent<Unit>().team != team)
                {
                    Vector3 diff = co.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = co;
                        distance = curDistance;
                    }
                }
            }
            else if (co.gameObject.tag.Equals("Building"))
            {
                if (co.gameObject.GetComponentInParent<Building>().team != team)
                {
                    Vector3 diff = co.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = co;
                        distance = curDistance;
                    }
                }
            }
        }
        if (closest != null)
        {
            nearestEnemy = closest.gameObject;
        }
        else
        {
            nearestEnemy = null;
        }
        checking = false;
        
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.SendMessage("AddDamage");
        }
    }

    public void changeColor(Color color)
    {
        if (unitName.Equals("VehicleTank"))
        {
            materials[2].color = color;
            materials[5].color = color;
            materials[7].color = color;
            materials[13].color = color;
            materials[15].color = color;
            materials[24].color = color;
        }
    }

    //////////////////////////////// SaveSystem ////////////////////////////////

    [System.Serializable]
    struct UnitSaveData
    {
        public UnitData unitData;
        public float timeSinceLastAttack;
        public int team;
        public Color teamColor;
        public Transform target;
        public float currentHealth;

        public UnitStates currentState;

        public float[] position;
    }

    public object CaptureState()
    {
        UnitSaveData data;
        data.unitData = unitData;
        data.timeSinceLastAttack = timeSinceLastAttack;
        data.team = team;
        data.teamColor = teamColor;
        data.target = target;
        data.currentHealth = objectLife.getHealth();

        data.currentState = currentState;

        data.position = new float[3];

        data.position[0] = transform.position.x;
        data.position[1] = transform.position.y;
        data.position[2] = transform.position.z;

        return data;
    }

    public void RestoreState(object dataLoaded)
    {
        UnitSaveData data = (UnitSaveData)dataLoaded;
        unitData = data.unitData;
        timeSinceLastAttack = data.timeSinceLastAttack;
        team = data.team;
        teamColor = data.teamColor;
        target = data.target;
        health = data.currentHealth;

        currentState = data.currentState;

        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
    }
}
