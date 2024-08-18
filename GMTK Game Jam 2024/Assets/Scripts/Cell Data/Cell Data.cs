using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CellData", menuName = "Cell Data")]
public class CellData : ScriptableObject
{
    public double Attack; // They attack cancer inside themselves and/or in a cell next to them
    public double Defense; 
    public double Mass; // if infected this determines how many cancerparticles that generates

    public CellType CellType
    {
        get
        {
            if (Attack > 60)
            {
                return CellType.Attacker;
            }
            else if (Defense > 60)
            {
                return CellType.Defender;
            }
            else
            {
                return CellType.Normal;
            }
        }
    }
}
public enum CellType
{
    Attacker,
    Defender,
    Normal
}