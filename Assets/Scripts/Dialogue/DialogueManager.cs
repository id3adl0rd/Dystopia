using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private GameObject _choicePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    [SerializeField] private GameObject[] _choices;
    [SerializeField] private List<Quest> _quests;
    private TextMeshProUGUI[] _choicesText;

    private Story _currentStory;
    public bool _dialogueIsPlaying { get; private set; }
    
    private static DialogueManager instance;

    private Coroutine _coroutine;

    private void Awake()
    {
        if (instance != null)
            Debug.Log("More than one dialogue manager!");
        
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Update()
    {
        if (!_dialogueIsPlaying) 
            return;
        
        if (_currentStory.currentChoices.Count == 0 && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ContinueStory();
        }
    }

    private void Start()
    {
        StartCoroutine(ExitDialogueMode());

        _choicesText = new TextMeshProUGUI[_choices.Length];
        int index = 0;
        foreach (var choice in _choices)
        {
            _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        _currentStory = new Story(inkJSON.text);
        _dialogueIsPlaying = true;
        _dialoguePanel.SetActive(true);
        _choicePanel.SetActive(true);
        _currentStory.BindExternalFunction("startQuest", (string value) =>
        {
            if (QuestController.instance.GetQuest() != null)
            {
                return "У тебя уже есть задание. Чего тебе еще надо?";
            }
                    
            QuestController.instance.SetQuest(_quests[Random.Range(0, _quests.Count)]);
            NotifyController.instance.AddToQueue("Новое задание!", 0f);
            
            return "Держи свое задание";
        });
        
        _currentStory.BindExternalFunction("closeDialogue", () =>
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            
            _coroutine = StartCoroutine(ExitDialogue());
        });

        ContinueStory();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        
        _coroutine = StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        
        _dialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        _dialogueText.text = "";
    }
    
    private IEnumerator ExitDialogue()
    {
        _choicePanel.SetActive(false);
        yield return new WaitForSeconds(5f);
        
        _dialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        _dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            _dialogueText.text = _currentStory.Continue();
            
            DisplayChoices();
        }
        else
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            
            _coroutine =  StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;

        if (currentChoices.Count > _choices.Length)
        {
            Debug.LogError("more choices than possible");
        }

        int index = 0;

        foreach (var choice in currentChoices)
        {
            _choices[index].gameObject.SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < _choices.Length; i++)
        {
            _choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        //EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        //EventSystem.current.SetSelectedGameObject(_choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
        
        if (_currentStory.currentChoices.Count == 0)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            
            _coroutine = StartCoroutine(ExitDialogue());
        }
    }
}