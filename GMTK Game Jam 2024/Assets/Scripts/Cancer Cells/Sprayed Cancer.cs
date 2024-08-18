using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class SprayedCancer : MonoBehaviour
{
    [SerializeField] public float _damage;
    [SerializeField] private float _lifeDuration;
    [SerializeField] private float _speed;

    private Rigidbody2D rigidbody;


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
            Cell normalCell = collision.gameObject.GetComponent<Cell>();
            Debug.Log(normalCell!=null);
            //Get component and infect
            if (normalCell != null && _damage > normalCell.cellData.Defense)
            {

            }
            else 
            {
                Debug.Log("Normal cell object not null: "+ (normalCell != null));
                Debug.Log("cancer cell damage ["+_damage+"]"+", normal cell defense [" +normalCell.cellData.Defense+"]");
            }
        }
        // else just bounce
    }

}
