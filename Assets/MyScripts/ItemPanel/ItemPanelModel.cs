using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using LuaInterface;
using LitJson;

public class ItemPanelModel : MonoBehaviour {
    
    public LuaFunction getJsonData = null;

    public object[] data;
    LuaTable tableObj = null;

    public List<object> tableDatas = new List<object>();


    //保存lua解析出来的属性

        [HideInInspector]
    public string itemPanelModel = @"
        ItemPanelModel = {};
        local this = ItemPanelModel;

        luanet.load_assembly('UnityEngine');
        
        cjson = require 'cjson'
        
        local capJsonData = nil;
        local clothesJsonData = nil;

        itemsList = {};
        itemsAttributeList = {};

        function ItemPanelModel:GetDataByJson(json)

            ItemPanelModel:ResetModel();


            local jsonData = cjson.decode(json);
            
            for i=1,#jsonData,1 do
                if i==1 then
                    --print(jsonData[i]['dataType']);
                elseif i==2 then
                    itemsList = jsonData[i]['Type'];
                    
                    for j=1 ,#itemsList,1 do

                        table.insert(itemsAttributeList,itemsList[j]);
                        
                    end
                    
                    
                end
            end
            
            
            return itemsAttributeList;
            
        end

        function ItemPanelModel:ResetModel()

            --重置model数据
            for i = #itemsList ,1 ,-1 do
                table.remove(itemsList, i);
            end

            for i = #itemsAttributeList ,1 ,-1 do
                table.remove(itemsAttributeList, i);
            end

        end

";
    

    public void GetJsonFile(string fileName)
    {


        TextAsset jsonStr = Resources.Load<TextAsset>("JsonData/" + fileName);
        //Debug.Log(jsonStr.ToString());
        getJsonData = LuaNew.lua.GetLuaFunction("ItemPanelModel.GetDataByJson");
        //第一个值是object，第二个值才能传递过去
        data = getJsonData.Call("", jsonStr.ToString());

        
        tableObj = (LuaTable)data[0];
        //print(tableObj.Count);
        for (int i = 0; i < tableObj.Count + 1; i++)
        {
            tableDatas.Add(tableObj[i]);

        }
        //Debug.Log("tableData.Count:" + tableDatas.Count);
    }
    
}
