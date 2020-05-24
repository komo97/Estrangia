using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonBehaviour : MonoBehaviour
{
    public int              index = 0;
    public ScriptableAction action;
    public WorldBehaviour   world;
    
    [SerializeField]
    private Image           _icon;

    [SerializeField]
    private Text            _title,
                            _description,
                            _influence;
    
    [SerializeField]
    private Button          _button;

    public ChoiceContainer  choiceContainer;

    private bool selected = false;
    // Start is called before the first frame update
    void Start()
    {
        _icon.sprite = action.image;
        _title.text = action.actionName;
        _description.text = action.description;
        _influence.text = action.influence.ToString();
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_button.interactable)
        {
            //if (world.acceptedEvents.Contains(action))
            //    _button.interactable = false;
        }
    }

    private void OnDestroy()
    {
        //if (selected != true)
        //    world.RemoveEventFromIndustry(action);
    }

    public void OnClickAction()
    {
        world.EventSelection(action,true);
        selected = true;
        choiceContainer.FinishSelection();
    }
}
