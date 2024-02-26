using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")] 
    [SerializeField] private GameObject visualCue;

    [Header("Dialogue JSON")] [SerializeField]
    private TextAsset _dialogueJSON;
    
    private bool _playerInRange;

    private void Awake()
    {
        _playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (_playerInRange && !DialogueManager.GetInstance()._dialogueIsPlaying)
            visualCue.SetActive(true);
        else
            visualCue.SetActive(false);
    }

    public void EnterDialogue()
    {
        DialogueManager.GetInstance().EnterDialogueMode(_dialogueJSON);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
            _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
            _playerInRange = false;
    }
}