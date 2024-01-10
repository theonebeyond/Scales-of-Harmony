using UnityEngine;
using System.Collections.Generic;

public class PowerManager : MonoBehaviour
{
    public GameObject dragon; // Reference to the main dragon
    private DragonData[] currentDragon;
    private List<Blessing> currentBlessing;
    public List<Blessing> AllBlessings; // List to hold blessings collection
    public GameObject blessingControlPrefab;
    public float attackInterval;

    void Start()
    {
        currentBlessing = new List<Blessing>();
    }

    public void AddBlessing(int BlessingNo)
    {
        currentBlessing.Add(AllBlessings[BlessingNo]);
        GameObject blessingControlObj = Instantiate(blessingControlPrefab);
        BlessingControl blessingControl = blessingControlObj.GetComponent<BlessingControl>();
        blessingControl.Initialize(AllBlessings[BlessingNo]);
        // Instantiate blessing object and add it to the blessings list
    }

    void Update()
    {

    }




    // Additional methods to activate, deactivate, or modify blessings
}