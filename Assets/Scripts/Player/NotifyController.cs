using UnityEngine;

public class NotifyController : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    public void AddToQueue(string text, float time)
    {
        _gameObject.GetComponent<PopupNotify>().AddToQueue(text);
    }
}