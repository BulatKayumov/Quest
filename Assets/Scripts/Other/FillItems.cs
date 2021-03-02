using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _Quest;

public class FillItems : MonoBehaviour
{
    public Room[] rooms;
    public void Fill()
    {
        Debug.Log("Fill");
        foreach(Room room in rooms)
        {
            foreach(Transform child in room.transform)
            {
                if(child.tag == "ThingPlacesRoot")
                {
                    room.itemPlaces = new List<ItemPlace>();
                    for(int i = 0; i < child.childCount; i++)
                    {
                        room.itemPlaces[i] = child.GetChild(i).GetComponent<ItemPlace>();
                    }
                }
            }
        }
    }
}
