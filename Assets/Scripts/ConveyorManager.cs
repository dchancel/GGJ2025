using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    private Conveyor[] conveyor;
    public bool checker;

    private void Start()
    {
        RefreshReferences();
    }

    private void OnValidate()
    {
        RefreshReferences();
    }

    private void RefreshReferences()
    {
        conveyor = transform.GetComponentsInChildren<Conveyor>();
        for(int i = 0; i < conveyor.Length; i++)
        {
            if(i + 1 < conveyor.Length)
            {
                conveyor[i].nextConveyor = conveyor[i + 1];
                conveyor[i].isServingWindow = false;
            }
            else
            {
                conveyor[i].isServingWindow = true;
            }
        }
    }
    
}
