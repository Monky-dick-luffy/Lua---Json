using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{



    private int itemId;
    private int itemNum;
    private string itemName = null;
    private string itemPath = null;

    public int Id { get { return itemId; } set { itemId = value; } }
    public int Num { get { return itemNum; } set { itemNum = value; } }
    public string Name { get { return itemName; } set { itemName = value; } }
    public string Path { get { return itemPath; } set { itemPath = value; } }

    public Item(int id,string name,int num,string path)
    {
        id = Id;
        name = Name;
        num = Num;
        path = Path;
        ToString();
    }

    public override string ToString()
    {
        return "Id:" + Id + ", Name:" + Name + ", Num:" + Num + ", Path:" + Path;
    }





}
