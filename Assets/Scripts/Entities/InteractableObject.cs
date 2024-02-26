using UnityEngine;

public class InteractableObject : CollidableObject
{
    private bool _isInteracted;
    
    protected override void OnCollided(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Player>())
        {
            if (collider.gameObject.GetComponent<Player>()._playerMovement.isInteract == true)
            {
                OnInteract();
            }
        }    
    }

    private void OnInteract()
    {
        if (_isInteracted == false)
        {
            _isInteracted = true;
            Debug.Log("was interacted");
        }
        else
        {
            _isInteracted = false;
            Debug.Log("no more");
        }
    }
}