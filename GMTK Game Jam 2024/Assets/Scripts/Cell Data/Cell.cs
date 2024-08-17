using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer _sr;
    public CellData cellData;
    public float checkRadius = 2f;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (cellData.CellType)
        {
            case CellType.Attacker:
                _sr.color = Color.blue;
                break;

            case CellType.Defender:
                _sr.color = Color.yellow;
                break;

            case CellType.Normal:
                _sr.color = Color.green;
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        Collider[] overlappingObjects = Physics.OverlapSphere(transform.position, checkRadius);

        if (overlappingObjects.Length > 0)
        {
            // There are overlapping objects
            foreach (Collider other in overlappingObjects)
            {
                if (other.gameObject != gameObject) // Avoid self-detection
                {
                    // Do something when objects overlap
                    var neighbour = other.GetComponent<Cell>().cellData;
                    if (neighbour.Defense > 0)
                        neighbour.Defense -= cellData.Attack * 0.1;

                }
            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);

    }
}
