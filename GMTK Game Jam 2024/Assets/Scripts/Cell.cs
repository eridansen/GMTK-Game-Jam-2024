using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer _sr;
    public CellData cellData;

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

}
