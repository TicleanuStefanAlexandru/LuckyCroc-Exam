using UnityEngine;

public class ATMTrigger : MonoBehaviour
{
    public ATMUI atmUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            atmUI.OpenATM();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            atmUI.CloseATM();
        }
    }
}