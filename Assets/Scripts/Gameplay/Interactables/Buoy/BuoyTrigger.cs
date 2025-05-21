using UnityEngine;
using UnityEngine.Events;

public class BuoyTrigger : MonoBehaviour
{
    public UnityEvent<DataCollectionManager.MachineType> BuoyreelTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buoy"))
        {
            BuoyreelTrigger.Invoke(DataCollectionManager.MachineType.Buoy);
            Debug.Log("Buoy Triggered");
        }
    }
}
