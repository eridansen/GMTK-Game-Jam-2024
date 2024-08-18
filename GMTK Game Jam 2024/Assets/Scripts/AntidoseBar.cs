using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntidoseBar : MonoBehaviour
{
    public Slider adBar;

    public int maxAD = 1000;
    public int AD;

    // Start is called before the first frame update
    void Start()
    {
        AD = maxAD;
        SetMaxAD(maxAD);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMaxAD(int AD)
    {
        adBar.maxValue = AD;
        adBar.value = AD;
    }

    public void SetAD(int AD)
    {
        adBar.value = AD;
    }

    public void LoseAD(int lostAD)
    {
        AD -= lostAD;
        if (AD < 0)
        {
            AD = 0;
        }
        SetAD(AD);
    }

    public void AddAD(int addedAD)
    {
        AD += addedAD;
        if (AD > maxAD)
        {
            AD = maxAD;
        }
        SetAD(AD);
    }
}
