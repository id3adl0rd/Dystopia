using System;
using System.Collections;
using Inventory.Model;
using UnityEngine;

public class Item : InteractableObject, IInteract
{
    [field: SerializeField] public ItemSO _inventoryItem { get; set; }
    [field: SerializeField] public int _quantity { get; set; } = 1;
    [field: SerializeField] private AudioSource _audioSource;
    [field: SerializeField] private float _duration = 0.3f;
    [field: SerializeField] private float _durationProtection = 0.5f;

    public void SetItem(ItemSO item = null, int quantity = 1)
    {
        GetComponent<SpriteRenderer>().sprite = item.ItemImage;
        _inventoryItem = item;
        _quantity = quantity;
    }
    
    private void Start()
    {
        if (_inventoryItem != null)
        {
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

    public void OnClick(Player _player, GameObject gameObject)
    {
        OnInteract(_player, gameObject);
    }

    protected override void OnInteract(Player _player, GameObject gameObject)
    {
        float minDist = .9f;
        float dist = Vector2.Distance(gameObject.transform.position, _player.gameObject.transform.position);
        
        if (minDist >= dist)
        {
            Item _item = gameObject.GetComponent<Item>();
            _player._inventoryController.AddItem(_item);  
        }
    }
}