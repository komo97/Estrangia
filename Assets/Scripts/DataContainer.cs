using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Gases
{
    public float    waterVapour,
                    carbonDioxide,
                    methane,
                    nitrousOxide,
                    CFCs;
    public Gases()
    {
        waterVapour = carbonDioxide = methane = nitrousOxide = CFCs = 0;
    }

    public Gases(float w, float c, float m, float n, float cfc)
    {
        waterVapour = w;
        carbonDioxide = c;
        methane = m;
        nitrousOxide = n;
        CFCs = cfc;
    }
    public Gases(Gases gases)
    {
        GeneratePercentage(gases);
    }

    public float TotalSum()
    {
        return waterVapour + carbonDioxide + methane + nitrousOxide + CFCs;
    }

    public void GeneratePercentage(Gases gases)
    {
        float tsum = gases.TotalSum();
        if (tsum == 0)
            return;
        waterVapour = HelperFuncs.RoundToDecimals((gases.waterVapour / tsum) * 100, 1);
        carbonDioxide = HelperFuncs.RoundToDecimals((gases.carbonDioxide / tsum) * 100, 1);
        methane = HelperFuncs.RoundToDecimals((gases.methane / tsum) * 100, 1);
        nitrousOxide = HelperFuncs.RoundToDecimals((gases.nitrousOxide / tsum) * 100, 1);
        CFCs = HelperFuncs.RoundToDecimals((gases.CFCs / tsum) * 100, 1);
    }

    public static Gases operator +(Gases a, Gases b)
    {
        return new Gases(a.waterVapour + b.waterVapour, a.carbonDioxide + b.carbonDioxide, a.methane + b.methane, a.nitrousOxide + b.nitrousOxide, a.CFCs + b.CFCs);
    }

    public static Gases operator -(Gases a, Gases b)
    {
        return new Gases(a.waterVapour - b.waterVapour, a.carbonDioxide - b.carbonDioxide, a.methane - b.methane, a.nitrousOxide - b.nitrousOxide, a.CFCs - b.CFCs);
    }

    public static Gases operator *(Gases a, Gases b)
    {
        return new Gases(a.waterVapour * b.waterVapour, a.carbonDioxide * b.carbonDioxide, a.methane * b.methane, a.nitrousOxide * b.nitrousOxide, a.CFCs * b.CFCs);
    }

    public static Gases operator *(Gases a, int b)
    {
        return new Gases(a.waterVapour * b, a.carbonDioxide * b, a.methane * b, a.nitrousOxide * b, a.CFCs * b);
    }

    public static Gases operator *(Gases a, float b)
    {
        return new Gases(a.waterVapour * b, a.carbonDioxide * b, a.methane * b, a.nitrousOxide * b, a.CFCs * b);
    }

    public static Gases operator /(Gases a, float b)
    {
        return new Gases(a.waterVapour / b, a.carbonDioxide / b, a.methane / b, a.nitrousOxide / b, a.CFCs / b);
    }

    public static Gases operator /(Gases a, Gases b)
    {
        return new Gases(a.waterVapour / b.waterVapour, a.carbonDioxide / b.carbonDioxide, a.methane / b.methane, a.nitrousOxide / b.nitrousOxide, a.CFCs / b.CFCs);
    }

    public override string ToString()
    {
        return "Water Vapour: " + waterVapour + "%\nCarbon Dioxide: " + carbonDioxide + "%\nMethane: " + methane + "%\nNitrous Oxide: " + nitrousOxide + "%\nCFCs: " + CFCs + "%";
    }

}


[Serializable]
public class Industry
{
    public string industryName;
    public Gases baseGenerationPerDay;
    public Gases baseMultiplierPerDay;
}

public enum IndustryTypes
{
    agriculture = 0,
    factory,
    IT,
    vehicle,
    city
}

[Serializable]
public class NaturalResource
{
    public string resourceName;
    public Gases baseReductionPerDay;
    public int ammount;
    public void OnDateUpdate(ref Gases gases)
    {
        gases -= ((baseReductionPerDay * ammount)) / 1000.0f;
    }
}

public class HelperFuncs
{
    public static float RoundToDecimals(float value, int decimalPoints)
    {
        int dec = 10 * decimalPoints;
        return Mathf.Ceil(value * dec) / dec;
    }
}

public class NodeBehaviour
{
    public int nodePopulation;
    public Industry industry;
    public float acceptancePerc;
    public int ammount;
    public List<ScriptableAction> pendingEvents = new List<ScriptableAction>();
    private int days = 0;

    public void OnDateUpdate(ref Gases gases)
    {
        Gases total = new Gases();
        days++; 
        days %= 30;
        foreach(ScriptableAction ev in pendingEvents)
        {
            if (days == 0)
            {
                float var = UnityEngine.Random.Range(0, 100);
                if (var <= acceptancePerc)
                {
                    ev.ammountAccepted = UnityEngine.Random.Range(ev.ammountAccepted, ammount + 1);
                }
            }
            total += ((industry.baseGenerationPerDay * ev.influence) * ev.ammountAccepted) / 1000.0f;
        }
        gases += (((industry.baseGenerationPerDay * industry.baseMultiplierPerDay) * ammount) / 1000.0f) + total;
    }

    public List<float> GetPercOfAcceptance()
    {
        List<float> values = new List<float>();
        foreach(ScriptableAction ev in pendingEvents)
        {
            values.Add(ev.ammountAccepted / ammount);
        }
        return values;
    }

    public List<int> GetAcceptanceValues()
    {
        List<int> values = new List<int>();
        foreach (ScriptableAction ev in pendingEvents)
        {
            values.Add(ev.ammountAccepted);
        }
        return values;
    }
}

[Serializable]
public class New
{
    public ScriptableAction action;
    public Sprite sprite;
    public Vector3Int displayDate;
    [HideInInspector]
    public bool posted;
}

[Serializable]
public class Dialogue
{
    public string character;
    [TextArea]
    public string dialogue;
}