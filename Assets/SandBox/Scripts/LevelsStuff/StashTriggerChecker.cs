using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StashTriggerChecker : MonoBehaviour
{
    private GameObject stashIndicator;
    private Stash stash;

    private void Start()
    {
        stashIndicator = FindObjectOfType<StashManager>().stashIndicator;
        stash = GetComponentInParent<Stash>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (stash.crashable)
        {
            if (other.CompareTag("Player"))
                stashIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (stash.crashable)
        {
            if (other.CompareTag("Player"))
                stashIndicator.SetActive(false);
        }
    }
}
