using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Security.Cryptography;

public class ChoiceContainer : MonoBehaviour
{
    public ScriptableAction[]       action;
    public Vector3Int               recurringTimeActivation;
    public WorldBehaviour           world;

    [SerializeField]
    private string                  _description;

    [SerializeField]
    private GameObject              _actionPrefab;

    [SerializeField]
    private HorizontalLayoutGroup   _layout;

    [SerializeField]
    private Text                    _descriptionText;

    [SerializeField]
    private Image                   _background;

    [SerializeField]
    private Sprite[]                _dialogue;

    [SerializeField]
    private Image                   _dialogueText;

    [SerializeField]
    private bool                    _unique;

    private DateTime                _dateTillActivation = new DateTime(1,1,1);
    private DateTime                _recTime;
    private bool                    _runOnce;

    private int                     _dialogueIndex;
    
    public void Start()
    {
        _runOnce = false;
        _recTime = new DateTime(recurringTimeActivation.x, recurringTimeActivation.y, recurringTimeActivation.z);
        _dialogueIndex = -1;
    }

    public void AdvanceTime()
    {
        _dateTillActivation = _dateTillActivation.AddDays(1);
    }

    private void Update()
    {

    }

    public bool ShouldActivate()
    {
        return (_recTime <= _dateTillActivation) && (!_unique || (_unique && !_runOnce));
    }

    public void ResetTime()
    {
        _dateTillActivation = new DateTime(1, 1, 1);
    }

    public void DoAction()
    {
        AdvanceTime();
        if(ShouldActivate())
        {
            _runOnce = true;
            ResetTime();
            world.stopTime = true;
            _dialogueText.gameObject.SetActive(true);
            _dialogueText.GetComponent<Button>().onClick.RemoveAllListeners();
            _dialogueText.GetComponent<Button>().onClick.AddListener(AdvanceText);
            AdvanceText();
        }
    }

    public void ChoiceStart()
    {
        _descriptionText.gameObject.SetActive(true);
        _background.gameObject.SetActive(true);
        _descriptionText.text = _description;
        _layout.gameObject.SetActive(true);
        GeneratePrefabs();
    }

    public void FinishSelection()
    {
        for(int i = 0; i < _layout.transform.childCount; ++i)
            Destroy(_layout.transform.GetChild(i).gameObject);
        _layout.gameObject.SetActive(false);
        _descriptionText.gameObject.SetActive(false);
        _background.gameObject.SetActive(false);
        world.stopTime = false;
    }

    public void GeneratePrefabs()
    {
        for(int i = 0; i < action.Length; ++i)
        {
            ActionButtonBehaviour button = Instantiate(_actionPrefab, _layout.transform).GetComponent<ActionButtonBehaviour>();
            button.action = action[i];
            button.index = i;
            button.world = world;
            button.choiceContainer = this;
        }
    }

    public void AdvanceText()
    {
        _dialogueIndex++;
        if (_dialogueIndex < _dialogue.Length)
            _dialogueText.sprite = _dialogue[_dialogueIndex];
        else
        {
            _dialogueText.gameObject.SetActive(false);
            ChoiceStart();
        }
    }
}
