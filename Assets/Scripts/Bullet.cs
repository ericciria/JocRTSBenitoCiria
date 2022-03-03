using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float m_movementSpeed;
    [SerializeField] float m_lifeSpan = 50.0f;
    [SerializeField] LayerMask m_damageableMask;

    public void Start()
    {
        Invoke(nameof(AutoDestroy), m_lifeSpan);
    }

    void Update()
    {
        Vector3 movementVector = transform.forward * m_movementSpeed * Time.deltaTime;

        transform.position = new Vector3(
            transform.position.x + movementVector.x,
            transform.position.y + movementVector.y,
            transform.position.z + movementVector.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Terrain")
        {
            Debug.Log("GroundHit");
        }
        Destroy(transform.parent.gameObject);
    }

    private void AutoDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
