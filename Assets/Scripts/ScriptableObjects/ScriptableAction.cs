using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/ScriptableAction", order = 1)]
public class ScriptableAction : ScriptableObject
{
    [SerializeField]
    public string               actionName;

    [SerializeField]
    [TextArea]
    public string               description;

    [SerializeField]
    public Gases                influence;

    [SerializeField]
    public Sprite               image;

    [SerializeField]
    public IndustryTypes[]      affectedIndustries;

    [SerializeField]
    public ScriptableAction[]   connectedEvents;
    
    [HideInInspector]
    public Transform            uiTransform;
    
    [HideInInspector]
    public int                  ammountAccepted = 0;

    public override bool Equals(object other)
    {
        if (other == null)
            return false;
        ScriptableAction nObj = other as ScriptableAction;
        if (nObj == null)
            return false;
        return this.actionName == nObj.actionName;
    }
}
