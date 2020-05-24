using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldBehaviour : MonoBehaviour
{
    public delegate void OnDayUpdate(ref Gases gases);

    [Header("Seed data")]
    [SerializeField]
    private Gases                   _gasesWeight;

    [SerializeField]
    private float                   _baseTemp;

    [SerializeField]
    private Vector3Int              _startingDate,
                                    _goalDate;


    [SerializeField]
    private float[]                 _secondsPerDay;

    [SerializeField]
    public NaturalResource[]        naturalResources;

    [SerializeField]
    public Industry[]               baseIndustries;

    [SerializeField]
    private ScriptableAction        initialEvent;

    [SerializeField]
    private CountryBehaviour[]      _countries;

    [SerializeField]
    private int                     _gasBreakPoint;

    [SerializeField]
    private New[]                   _news;

    [Header("UI")]
    [SerializeField]
    Text                            _dateDisplay;
    [SerializeField]
    Text                            _waterVapourCount,
                                    _carbonDioxideCount,
                                    _methaneCount,
                                    _nitrousOxideCount,
                                    _CFCsCount,
                                    _tempText;

    [SerializeField]
    Slider                          _waterVapourSlider,
                                    _carbonDioxideSlider,
                                    _methaneSlider,
                                    _nitrousOxideSlider,
                                    _CFCsSlider,
                                    _tempSlider;

    [SerializeField]
    GameObject                      _eventPrefab,
                                    _newsObject,
                                    _gameOverBad,
                                    _gameOverGood;

    [SerializeField]
    Transform                       _technologyParent;

    [SerializeField]
    ChoiceContainer[]               _choicePrefabs;

    private float                   _time = 0, 
                                    _temp = 0;
    private int                     _secondsPerDayIndex = 0;

    private DateTime                _date, 
                                    _goalDateT;
    
    [HideInInspector]
    public static OnDayUpdate       onDayUpdate; 
    
    [HideInInspector]
    public List<ScriptableAction>   acceptedEvents = new List<ScriptableAction>();
    
    [HideInInspector]
    public List<ScriptableAction>   unManagedEvents = new List<ScriptableAction>();

    [HideInInspector]
    public bool stopTime = false;

    // Start is called before the first frame update
    void Start()
    {
        _date = new DateTime(_startingDate.x, _startingDate.y, _startingDate.z);
        _goalDateT = new DateTime(_goalDate.x, _goalDate.y, _goalDate.z);
        //EventSelection(initialEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTime)
        {
            AdvanceDay();
        }
        UpdateUI();
    }

    private void AdvanceDay()
    {
        _time += Time.deltaTime;
        if (_time >= _secondsPerDay[_secondsPerDayIndex])
        {
            _time = 0;
            int py = _date.Year;
            int pm = _date.Month;
            _date = _date.AddDays(1);
            onDayUpdate(ref _gasesWeight);
            Mathf.Clamp(_gasesWeight.carbonDioxide, 100, 1000000);
            Mathf.Clamp(_gasesWeight.methane, 1000, 1000000);
            Mathf.Clamp(_gasesWeight.nitrousOxide, 1, 1000000);
            Mathf.Clamp(_gasesWeight.CFCs, 0, 1000000);
            if (py != _date.Year)
                AdvanceYear();
            if (pm != _date.Month)
                AdvanceMonth();
            AdvanceValues();
            foreach (ChoiceContainer choice in _choicePrefabs)
                choice.DoAction();
        }
    }

    public void PostNews()
    {
        foreach(New n in _news)
        {
            DateTime t = new DateTime(n.displayDate.x, n.displayDate.y, n.displayDate.z);
            if (acceptedEvents.Contains(n.action) && !n.posted && _date >= t)
            {
                _newsObject.SetActive(true);
                _newsObject.transform.GetChild(0).GetComponent<Image>().sprite = n.sprite;
                n.posted = true;
                stopTime = true;
                break;
            }
        }
    }

    private void AdvanceYear()
    {

    }

    private void AdvanceMonth()
    {
        PostNews();
        switch (_date.Month)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
        }
    }

    void AdvanceValues()
    {
        _temp = _gasesWeight.TotalSum() / _gasBreakPoint;
        if (_temp >= 5)
            GameOver(false);
        if(_date >= _goalDateT)
            GameOver(true);
    }

    public void EventSelection(int a)
    {
        acceptedEvents.Add(acceptedEvents[acceptedEvents.Count - 1].connectedEvents[a]);
        _technologyParent = acceptedEvents[acceptedEvents.Count - 1].uiTransform;
        if (acceptedEvents.Count > 1)
        {
            foreach (ScriptableAction ev in acceptedEvents[acceptedEvents.Count - 2].connectedEvents)
            {
                if (acceptedEvents[acceptedEvents.Count - 1] != ev)
                {
                    unManagedEvents.Add(ev);
                }
            }
        }

        for(int i = 0; i < acceptedEvents[acceptedEvents.Count - 1].connectedEvents.Length; ++i)
        {
            ActionButtonBehaviour button = Instantiate(_eventPrefab, _technologyParent).GetComponent<ActionButtonBehaviour>();
            button.action = acceptedEvents[acceptedEvents.Count - 1].connectedEvents[i];
            button.index = i;
            button.world = this;
        }
        AddEventToIndustry(acceptedEvents[acceptedEvents.Count - 1]);
    }

    public void EventSelection(ScriptableAction eve, bool decision = false)
    {
        acceptedEvents.Add(eve);
        if (!decision)
        {
            _technologyParent = acceptedEvents[acceptedEvents.Count - 1].uiTransform;
            if (acceptedEvents.Count > 1)
            {
                foreach (ScriptableAction ev in acceptedEvents[acceptedEvents.Count - 2].connectedEvents)
                {
                    if (acceptedEvents[acceptedEvents.Count - 1] != ev)
                    {
                        unManagedEvents.Add(ev);
                    }
                }
            }
            for (int i = 0; i < acceptedEvents[acceptedEvents.Count - 1].connectedEvents.Length; ++i)
            {
                ActionButtonBehaviour button = Instantiate(_eventPrefab, _technologyParent).GetComponent<ActionButtonBehaviour>();
                button.action = acceptedEvents[acceptedEvents.Count - 1].connectedEvents[i];
                button.index = i;
                button.world = this;
            }
        }
        AddEventToIndustry(acceptedEvents[acceptedEvents.Count - 1]);
    }

    void AddEventToIndustry(ScriptableAction ev)
    {
        foreach(IndustryTypes industry in ev.affectedIndustries)
        {
            foreach(CountryBehaviour country in _countries)
            {
                ScriptableAction nev = ScriptableObject.CreateInstance<ScriptableAction>();
                nev.actionName = ev.actionName;
                nev.description = ev.description;
                nev.influence = new Gases();
                nev.influence += ev.influence;
                nev.image = ev.image;
                nev.affectedIndustries = ev.affectedIndustries;
                nev.connectedEvents = ev.connectedEvents;
                nev.uiTransform = ev.uiTransform;
                nev.ammountAccepted = ev.ammountAccepted;
                country._nodes[(int)industry].pendingEvents.Add(nev);
            }
        }
    }

    public void RemoveEventFromIndustry(ScriptableAction ev)
    {
        foreach (IndustryTypes industry in ev.affectedIndustries)
        {
            foreach (CountryBehaviour country in _countries)
            {
                country._nodes[(int)industry].pendingEvents.Remove(ev);
            }
        }
        acceptedEvents.Remove(ev);
    }

    void UpdateUI()
    {
        Gases perc = new Gases(_gasesWeight);
        _waterVapourCount.text = perc.waterVapour + "%";
        _carbonDioxideCount.text = perc.carbonDioxide + "%";
        _methaneCount.text = perc.methane + "%";
        _nitrousOxideCount.text = perc.nitrousOxide + "%";
        _CFCsCount.text = perc.CFCs + "%";
        _waterVapourSlider.value = _gasesWeight.waterVapour;
        _carbonDioxideSlider.value = _gasesWeight.carbonDioxide;
        _methaneSlider.value = _gasesWeight.methane;
        _nitrousOxideSlider.value = _gasesWeight.nitrousOxide;
        _CFCsSlider.value = _gasesWeight.CFCs;
        _dateDisplay.text = _date.Year + "Y " + _date.Month + "M " + _date.Day + "D";
        _tempSlider.value = _temp;
        _tempText.text = HelperFuncs.RoundToDecimals(_temp,2).ToString() + "°";
    }

    public void SetSPDIndex(int val)
    {
        _secondsPerDayIndex = val;
    }

    public void GameOver(bool win)
    {
        stopTime = true;
        if (win)
        {
            _gameOverGood.SetActive(true);
        }
        else
        {
            _gameOverBad.SetActive(true);
        }
    }

    public void StartTime()
    {
        stopTime = false;
    }

    public void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
