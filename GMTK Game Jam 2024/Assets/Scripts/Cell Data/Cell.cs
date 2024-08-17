using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer _sr;
    public CellData cellData;
    public float checkRadius = 2f;
    public bool IsInfected;

    public double Attack
    {
        get
        {
            return cellData.Attack;
        }
    }

    public double Defense
    {
        get
        {
            return cellData.Defense;
        }
    }

    public double Mass
    {
        get
        {
            return cellData.Mass;
        }
    }
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        AttackOtherCells();
        ChangeColor();
    }

    private void ChangeColor()
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
        if (IsInfected)
        {
            _sr.color = Color.red;
        }
    }

    private void AttackOtherCells()
    {
        if (!IsInfected) return;
        Collider2D[] overlappingObjects = Physics2D.OverlapCircleAll(transform.position, checkRadius);
        Debug.Log(overlappingObjects.Length);
        if (overlappingObjects.Length > 0)
        {
            // There are overlapping objects
            foreach (Collider2D other in overlappingObjects)
            {
                if (other.gameObject != gameObject) // Avoid self-detection
                {
                    Cell cell = other.gameObject.GetComponent<Cell>();
                    Debug.Log(cell.Defense);
                    if (!cell.IsInfected && Attack > cell.Defense)
                    {
                        StartCoroutine(InfectCo(cell));
                    }
                }
            }
        }
    }

    IEnumerator InfectCo(Cell cell)
    {
        Debug.Log("infected");
        yield return new WaitForSeconds(2f);
        cell.IsInfected = true;

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);

    }
}
