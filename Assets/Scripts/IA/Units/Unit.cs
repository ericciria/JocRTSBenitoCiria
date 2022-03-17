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
    public string team;

    public NavMeshAgent agent;
    public Transform target;
    //public PlayerUnitStates currentState = new PlayerIdleState();
    private bool checking;

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

        objectLife.setMaxHealth(maxHealth);
        objectLife.setHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
