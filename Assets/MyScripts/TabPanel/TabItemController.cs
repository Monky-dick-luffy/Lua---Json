using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TabItemController : MonoBehaviour {

    Toggle toggle = null;

    ItemPanelController itemPanelCtrl = null;

    private void Awake()
    {
        itemPanelCtrl = GameObject.Find("ItemsPanel").GetComponent<ItemPanelController>();
        toggle = gameObject.GetComponent<Toggle>();
    }
    

    public void OnTabChose()
    {
        //加载jsonData
        if (GameObject.Find("TabsPanel/Mask/Tab_1").GetComponent<Toggle>().isOn == true)
        {
            
            itemPanelCtrl.GetJsonNameCallFunc("capsInformation");

        }
        else if (GameObject.Find("TabsPanel/Mask/Tab_2").GetComponent<Toggle>().isOn == true)
        {
            
            itemPanelCtrl.GetJsonNameCallFunc("clothesInformation");

        }
        

    }

}
