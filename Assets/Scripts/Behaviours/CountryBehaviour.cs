using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryBehaviour : MonoBehaviour
{
    [SerializeField]
    private string              _countryName;
    [SerializeField]
    private int[]               _baseIndustryNumber;
    [SerializeField]
    private int[]               _industryNumberVariations;
    
    public int[]                _naturalResources,
                                _naturalResourcesVariation;

    [SerializeField]
    private Sprite              _countryImage;

    [SerializeField]
    private WorldBehaviour      _baseWorld;

    public NodeBehaviour[]      _nodes;
    public NaturalResource[]    _natRes;

    [SerializeField]
    private int _totalIndusty = 0, _totalNaturalResources = 0;
    // Start is called before the first frame update
    void Start()
    {
        _nodes = new NodeBehaviour[_baseIndustryNumber.Length - 1];
        _natRes = new NaturalResource[_naturalResources.Length];
        for(int i = 0; i < _baseIndustryNumber.Length - 1; ++i)
        {
            int var = _baseIndustryNumber[i] + (Random.Range(-_industryNumberVariations[i], _industryNumberVariations[i]));
            _totalIndusty += var;
            _nodes[i] = new NodeBehaviour();
            _nodes[i].ammount = var;
            _nodes[i].acceptancePerc = _baseIndustryNumber[5] + (Random.Range((float)-_industryNumberVariations[5], (float)_industryNumberVariations[5]));
            _nodes[i].industry = new Industry();
            _nodes[i].industry.industryName = _baseWorld.baseIndustries[i].industryName;
            _nodes[i].industry.baseGenerationPerDay = _baseWorld.baseIndustries[i].baseGenerationPerDay;
            _nodes[i].industry.baseMultiplierPerDay = _baseWorld.baseIndustries[i].baseMultiplierPerDay;
            WorldBehaviour.onDayUpdate += _nodes[i].OnDateUpdate;
        }
        for(int i = 0; i < _naturalResources.Length; ++i)
        {
            int var = _naturalResources[i] + (Random.Range(-_naturalResourcesVariation[i], _naturalResourcesVariation[i])); 
            _totalNaturalResources += var;
            _natRes[i] = new NaturalResource();
            _natRes[i].ammount = var;
            _natRes[i].resourceName = _baseWorld.naturalResources[i].resourceName;
            _natRes[i].baseReductionPerDay = _baseWorld.naturalResources[i].baseReductionPerDay;
            WorldBehaviour.onDayUpdate += _natRes[i].OnDateUpdate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
