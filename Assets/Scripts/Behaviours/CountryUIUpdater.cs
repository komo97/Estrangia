using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryUIUpdater : MonoBehaviour
{
    [SerializeField]
    CountryBehaviour    _country;

    [SerializeField]
    Text                _text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = _country.name + "\n";
        foreach(NodeBehaviour node in _country._nodes)
        {
            _text.text += "\t" + node.industry.industryName + "\n";
            _text.text += "\tBase\n";
            _text.text += "\t\t Dioxido de carbono: " + node.industry.baseGenerationPerDay.carbonDioxide + "\n";
            _text.text += "\t\t Metano: " + node.industry.baseGenerationPerDay.methane + "\n";
            _text.text += "\t\t Oxido nitroso: " + node.industry.baseGenerationPerDay.nitrousOxide + "\n";
            _text.text += "\t\t CFCs: " + node.industry.baseGenerationPerDay.CFCs + "\n";
            _text.text += "\tGeneración\n";
            float cd = 0, m = 0, on = 0, cfcs = 0;
            cd = node.industry.baseMultiplierPerDay.carbonDioxide;
            m = node.industry.baseMultiplierPerDay.methane;
            on = node.industry.baseMultiplierPerDay.nitrousOxide;
            cfcs = node.industry.baseMultiplierPerDay.CFCs;
            foreach (ScriptableAction action in node.pendingEvents)
            {
                cd += action.influence.carbonDioxide;
                m += action.influence.methane;
                on += action.influence.nitrousOxide;
                cfcs += action.influence.CFCs;
            }
            _text.text += "\t\t Dioxido de carbono: " + cd + "\n";
            _text.text += "\t\t Metano: " + m + "\n";
            _text.text += "\t\t Oxido nitroso: " + on + "\n";
            _text.text += "\t\t CFCs: " + cfcs + "\n";
            _text.text += "\tAcciones\n";
            foreach(ScriptableAction action in node.pendingEvents)
            {
                _text.text += "\t\t" + action.actionName + "\n";
                _text.text += "\t\t\t % Aceptado: " + HelperFuncs.RoundToDecimals(((float)action.ammountAccepted / (float)node.ammount) * 100.0f, 2);
            }
        }
    }
}
