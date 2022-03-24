using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{

    [SerializeField] UnitData unitData;

    public string unitName;
    public string description;
    public Sprite icon;
    //[SerializeField] private int moneyCost;
    //[SerializeField] private int metalCost;
    private int attackDamage;
    public string typeOfUnit;
    private string damageMultiplierType;
    private int damageMultiplierAmount;
    private int maxHealth;
    private float movementSpeed;
    private float attackDistance;
    private float attackCooldown;
    public float timeSinceLastAttack = Mathf.Infinity;
    public GameObject nearestEnemy;

    public ObjectLife objectLife;
    public int team;
    public Color teamColor;

    public NavMeshAgent agent;
    public Transform target;
    //public PlayerUnitStates currentState = new PlayerIdleState();
    private bool checking;
    private bool constructor;

    Vector3 previousCorner;
    Vector3 currentCorner;



    private void Awake()
    {
        objectLife = GetComponent<ObjectLife>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        unitName = unitData.UnitName;
        description = unitData.Description;
        icon = unitData.Icon;


        attackDamage = unitData.AttackDamage;
        typeOfUnit = unitData.TypeOfUnit;
        damageMultiplierType = unitData.DamageMultiplierType;
        damageMultiplierAmount = unitData.DamageMultiplierAmount;
        maxHealth = unitData.MaxHealth;
        movementSpeed = unitData.MovementSpeed;
        attackDistance = unitData.AttackDistance;
        attackCooldown = unitData.AttackCooldown;
        constructor = unitData.Constructor;

        agent.speed = unitData.MovementSpeed;
        agent.acceleration = agent.speed - 2;

        objectLife.setMaxHealth(maxHealth);
        objectLife.setHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setObjective(RaycastHit hit, float offset)
    {
        if (!constructor && (hit.collider.gameObject.tag.Equals("Unit") && hit.collider.gameObject.GetComponent<Unit>().team != team) || (hit.collider.gameObject.tag.Equals("Construction") /*&& hit.collider.gameObject.GetComponent<Construction>().team != team*/))
        {
            target = hit.collider.gameObject.transform;
        }
        else if(constructor && (hit.collider.gameObject.tag.Equals("Construction") && hit.collider.gameObject.GetComponent<Unit>().team == team))
        {
            target = hit.collider.gameObject.transform;
        }
        else
        {
            target = null;
            agent.SetDestination(hit.point + new Vector3(offset, 0, 0));
        }
    }

    void OnDrawGizmos()
    {
        /*if (currentState.ToString().Equals("PlayerIdleState"))
        {
            Gizmos.color = Color.blue;
            //Debug.Log("AAAAAAAAAAAAAAA");
        }
        else if (currentState.ToString().Equals("PlayerPursueState"))
        {
            Gizmos.color = Color.yellow;
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
        }*/
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        DrawCircle(this.transform.position, this.transform.forward, attackDistance, 20);
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
        gos = GameObject.FindGameObjectsWithTag("Attackable");
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
}
