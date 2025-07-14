using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    public static GardenManager Instance {get; private set;}
    
    public List<FruitObject> fruits;
    
    private List<PlantSlot> _plantSlots;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        _plantSlots = GetComponentsInChildren<PlantSlot>().ToList();
    }

    public PlantSlot GrowAvailablePlantSlot(FruitObject fruit)
    {
        for (int i = 0; i < _plantSlots.Count; i++)
        {
            if (_plantSlots[i]._doneGrowing && _plantSlots[i]._currentFruitObject.type == FruitObject.Type.Nothing)
            {
                return _plantSlots[i];
            }
        }

        return null;
    }
}
