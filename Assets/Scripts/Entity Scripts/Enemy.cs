using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private GameObject bloodOrbPrefab;
    [SerializeField] private int bloodOrbsOnDeath;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void HandlePlayerTargeting()
    {
        
    }

    public override IEnumerator DeathSequence()
    {
        yield return null;
    }
}
