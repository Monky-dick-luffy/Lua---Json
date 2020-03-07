using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using LitJson;

[RequireComponent(typeof(ItemPanelModel))]
[RequireComponent(typeof(ItemPanelView))]

public class ItemPanelController : MonoBehaviour {

    List<object> tableData = null;

    private LuaFunction ctrlStart = null;
    public LuaFunction getDataInitFunc = null;
    public LuaFunction resetPanel = null;

    ItemPanelModel itemPanelModel = null;


    private string itemPanelCtrl = @"
        ItemPanelCtrl = {};
        local this = ItemPanelCtrl;

        luanet.load_assembly('UnityEngine');

        Behaviour = UnityEngine.Behaviour;
        GameObject = UnityEngine.GameObject;
        Transform = UnityEngine.Transform;
        RectTransform = UnityEngine.RectTransform;
        Resources = UnityEngine.Resources;
        Application = UnityEngine.Application;
        
        
        Item = UnityEngine.Item;

        Text = UnityEngine.UI.Text;
        
        itemList = {}
        slotList = {}
        itemNumList = {}
        
        local itemSlot = nil;
        local mask = nil;
        local itemNum = nil;
        
        function ItemPanelCtrl:Start()
            mask = GameObject.Find('ItemsPanel/Mask');
            itemSlot = Resources.Load('ItemSlot');
            ItemPanelCtrl:CreateItemslot();

        end
        
        --生成物品槽
        function ItemPanelCtrl:CreateItemslot()
           
            for i = 1 ,25 ,1 do
                local go = GameObject.Instantiate(itemSlot,Vector3.zero,Quaternion.identity,mask.transform);
                go.name = 'ItemSlot' .. i;
                table.insert(slotList,go);

                itemNum = go.Find('itemNum');
                table.insert(itemNumList,itemNum);
                itemNum:SetActive(false);
            end
            
        end

        --获取jsonData对象数组，生成物品，i表示生成顺序
        function ItemPanelCtrl:GetDataInitItem(id,name,num,path,i)
            --print(path)

            local itemPre = Resources.Load(path);
            local objPrefab = GameObject.Instantiate(itemPre,Vector3.zero,Quaternion.identity,slotList[i].transform);
            
            objPrefab:GetComponent('RectTransform').localPosition = Vector3.zero;
            
            table.insert(itemList,objPrefab);
            

            objPrefab.name = name .. '_' .. id;
            --print(GameObject.Find('ItemsPanel/Mask/ItemSlot' .. i .. '/itemNum').name)
            --显示数字
            GameObject.Find('ItemsPanel/Mask/ItemSlot' .. i .. '/itemNum'):SetActive(true);
            GameObject.Find('ItemsPanel/Mask/ItemSlot' .. i .. '/itemNum'):GetComponent('Text').text = num;
            
            --itemNumList[i]:SetActive(true);
            --itemNumList[i]:GetComponent('Text').text = num;

        end
        
        function ItemPanelCtrl:ResetItemPanel()
            
            if(#itemList > 0) then
                for i = #itemList, 1, -1 do

                    GameObject.Destroy(itemList[i]);
                    itemNumList[i]:SetActive(false);
                
                    table.remove(itemList,i);

                end
            end
        end
        
";
    
    void Awake()
    {
        LuaNew.lua.Start();
        itemPanelModel = new ItemPanelModel();
        LuaNew.lua.DoString(itemPanelModel.itemPanelModel);
        LuaNew.lua.DoString(itemPanelCtrl);

        
        ctrlStart = LuaNew.lua.GetLuaFunction("ItemPanelCtrl.Start");
        getDataInitFunc = LuaNew.lua.GetLuaFunction("ItemPanelCtrl.GetDataInitItem");
        resetPanel = LuaNew.lua.GetLuaFunction("ItemPanelCtrl.ResetItemPanel");

        
    }



    void Start () {
        //处理ctrl脚本
        ctrlStart.Call();

        
    }
    

    public void GetJsonNameCallFunc(string name)
    {

        if(tableData != null)
        {
            tableData.Clear();
        }

        resetPanel.Call();
        itemPanelModel.GetJsonFile(name);
        
        //处理model脚本
        tableData = itemPanelModel.tableDatas;//Items对象数据
        //print(tableData.Count + "¿");
        for (int i = 1; i < tableData.Count; i++)
        {
            //print(i);
            LuaTable table = (LuaTable)tableData[i];
            //print(table["Id"] + "...?");
            getDataInitFunc.Call("", table["Id"], table["Name"], table["Num"], table["Path"], i);
        }


    }


}
