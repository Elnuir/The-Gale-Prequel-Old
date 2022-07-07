using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StashManager : MonoBehaviour
{
    private Stash[] stashes;
    public GameObject stashIndicator;
    void Start()
    {
        stashes = FindObjectsOfType<Stash>();
        if (stashes == null) return;
        stashes[Random.Range(0, stashes.Length)].crashable = true;
        stashIndicator = GameObject.Find("StashIndicator");
        stashIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
