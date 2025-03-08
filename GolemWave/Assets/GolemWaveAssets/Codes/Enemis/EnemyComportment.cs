using UnityEngine;

public class EnemyComportment : MonoBehaviour
{
    Transform player;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.SqrMagnitude(transform.position - player.position) > 10 * 10) return;

        Vector3 posToPlayer = player.transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(posToPlayer), 20f*Time.deltaTime);

        if (posToPlayer.sqrMagnitude <= 2 * 2) return;
            rb.MovePosition(transform.position + posToPlayer.normalized * 20f * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, 10);
    }
}
