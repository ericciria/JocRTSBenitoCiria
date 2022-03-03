using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

    [SerializeField] Camera r_playerCamera;
    [SerializeField] GameObject r_bulletPrefab;
    [SerializeField] float m_interactionRange = 5.0f;
    [SerializeField] LayerMask m_interactionMask;
    [SerializeField] float m_dfireRate;
    [SerializeField] GameObject gameOver, victory;
    private Rigidbody rb;
    private GameObject bulletPointer;
    public int score;

    private float timeSinceLastShot = Mathf.Infinity;



    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        gameOver.SetActive(false);
        victory.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody>();
        bulletPointer = GameObject.Find("BulletPointer");
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            HandleShootingBullet();
        }
        if (Input.GetKey(KeyCode.E))
        {
            HandleInteraction();
        }
        if(score == 5)
        {
            bulletPointer.SetActive(false);
            victory.SetActive(true);
        }
    }

    private void HandleInteraction()
    {
        Ray ray;

        ray = r_playerCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_interactionRange, m_interactionMask))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                interactable.OnInteract();
            }

        }
    }

    private void HandleShootingBullet()
    {
        if (timeSinceLastShot >= m_dfireRate)
        {
            timeSinceLastShot = 0;
            Ray ray;
            ray = r_playerCamera.ScreenPointToRay(Input.mousePosition);

            Vector3 shootDirection = ray.direction;
            Vector3 shootInitialPosition = r_playerCamera.transform.position + shootDirection;
            Quaternion rotation = Quaternion.LookRotation(shootDirection);

            Instantiate(r_bulletPrefab, shootInitialPosition, rotation);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Death")
        {
            bulletPointer.SetActive(false);
            gameOver.SetActive(true);
        }
    }

}
