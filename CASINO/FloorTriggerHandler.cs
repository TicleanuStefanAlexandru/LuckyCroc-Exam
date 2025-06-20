using UnityEngine;

public class FloorTriggerHandler : MonoBehaviour
{
    public CasinoFloorEntranceManager entranceManager;

    public enum FloorType
    {
        BeggarsPit = 0,
        RabbitsField = 1,
        WolfsDen = 2,
        LionsArena = 3
    }

    public FloorType floor;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        switch (floor)
        {
            case FloorType.BeggarsPit:
                entranceManager.SetEnterMethod(entranceManager.EnterBeggarsPit, "Beggar's Pit");
                break;
            case FloorType.RabbitsField:
                entranceManager.SetEnterMethod(entranceManager.EnterRabbitsField, "The Rabbit's Field");
                break;
            case FloorType.WolfsDen:
                entranceManager.SetEnterMethod(entranceManager.EnterWolfsDen, "The Wolf's Den");
                break;
            case FloorType.LionsArena:
                entranceManager.SetEnterMethod(entranceManager.EnterLionsArena, "The Lion's Arena");
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        entranceManager.HideEntrancePopup();
    }
}
