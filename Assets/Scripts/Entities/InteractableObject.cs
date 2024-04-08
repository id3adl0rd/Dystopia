using UnityEngine;

public class InteractableObject : CollidableObject
{
    protected override void OnCollided(Collider2D collider)
    {
    }

    protected virtual void OnInteract(Player _player, GameObject gameObject)
    {
    }
}