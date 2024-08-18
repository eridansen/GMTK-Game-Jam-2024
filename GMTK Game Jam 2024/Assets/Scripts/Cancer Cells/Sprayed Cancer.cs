using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class SprayedCancer : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _lifeDuration;
    [SerializeField] private float _speed;

    private Rigidbody2D rigidbody;

    public UnityAction OnCollisionWithNormalCell;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Vector3 direction = new Vector3 (
            Random.Range(-1,1f),
            Random.Range(-1, 1f),
            0f);

        rigidbody.AddForce(direction * _speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("NormalCell"))
        {
            //Get component and infect
            OnCollisionWithNormalCell?.Invoke();
        }
        // else just bounce
    }
}
