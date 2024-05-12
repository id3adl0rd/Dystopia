using System;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    private Collider2D zCollider;
    private ContactFilter2D zFilter;
    private List<Collider2D> zCollidedObjects = new List<Collider2D>(1);

    protected virtual void Awake()
    {
        zCollider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        zCollider.OverlapCollider(zFilter, zCollidedObjects);
        foreach (var obj in zCollidedObjects)
        {
            OnCollided(obj);
        }
    }
    protected virtual void OnCollided(Collider2D collider)
    {
    }
}