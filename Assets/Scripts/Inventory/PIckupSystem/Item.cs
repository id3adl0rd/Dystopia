using System;
using System.Collections;
using Inventory.Model;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField] public ItemSO _inventoryItem { get; set; }
    [field: SerializeField] public int _quantity { get; set; } = 1;
    [field: SerializeField] private AudioSource _audioSource;
    [field: SerializeField] private float _duration = 0.3f;

    public void SetItem(ItemSO item = null)
    {
        GetComponent<SpriteRenderer>().sprite = item.ItemImage;
        _inventoryItem.ItemImage = item.ItemImage;
        _inventoryItem.Description = item.Description;
        _inventoryItem.Name = item.Name;
    }
    
    private void Start()
    {
        if (_inventoryItem != null)
        {
            Debug.Log("dsadadcxzcdsa");
            GetComponent<SpriteRenderer>().sprite = _inventoryItem.ItemImage;
        }
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    private IEnumerator AnimateItemPickup()
    {
        _audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < _duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / _duration);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}