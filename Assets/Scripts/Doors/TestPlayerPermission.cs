using System;
using UnityEngine;

public class TestPlayerPermission : MonoBehaviour, IPermission
{
    public bool CanOpenRestricted;
    
    public bool CanOpenDoor(DoorType doorType)
    {
        return doorType switch
        {
            DoorType.Public => true,
            DoorType.Locked => false,
            DoorType.Restricted => CanOpenRestricted,
            _ => throw new ArgumentOutOfRangeException(nameof(doorType), doorType, null)
        };
    }
}