using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalPoolScript : MonoBehaviour
{
    public int amountToPool;
    public static DecalPoolScript SharedInstanceDec;
    private GameObject[] decal;
    private int number = 0;

    void Awake()
    {
        SharedInstanceDec = this;
    }

    private void Start()
    {
        decal = new GameObject[amountToPool];
    }

    public void addDecal(GameObject dec)
    {
        if(decal[number] != null)
        {
            Destroy(decal[number]);
        }
        decal[number] = dec;
        number++;
        if(number >= amountToPool)
        {
            number = 0;
        }
    }
}
