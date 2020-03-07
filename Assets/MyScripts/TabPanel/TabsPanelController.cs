using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class TabsPanelController : MonoBehaviour {
    
    LuaFunction tabsItemLuaStart = null;

    string tabsPanelController = @"
        
        local tabItem = nil;
        local mask = nil;
        local tabs = {};
        
        luaTabsPanelController = {};
        local this = luaTabsPanelController;

        luanet.load_assembly('UnityEngine');
        
        GameObject = UnityEngine.GameObject;
        Resources = UnityEngine.Resources;
        
        function luaTabsPanelController:Start()
            mask = GameObject.Find('TabsPanel/Mask');
            tabItem = Resources.Load('TabItem');
            
            --生成所有tab按钮
            for i = 1,2,1 do
                tabs[i] = GameObject.Instantiate(tabItem,Vector3.zero,Quaternion.identity,mask.transform);
                tabs[i].name = 'Tab_' .. i;

                if i == 1 then
                    tabs[i].Find('Tab_1/Text'):GetComponent('Text').text = '帽子';
                elseif i == 2 then
                    tabs[i].Find('Tab_2/Text'):GetComponent('Text').text = '衣服';
                end

                tabs[i]:GetComponent('Toggle').isOn = false;
                tabs[i]:GetComponent('Toggle').group = mask:GetComponent('ToggleGroup');
            end
            --高亮第一个选项
            tabs[1]:GetComponent('Toggle').isOn = true;
            tabs[1]:GetComponent('Toggle'):Select();
            
        end
       ";
    

	void Start () {
        LuaNew.lua.DoString(tabsPanelController);
        tabsItemLuaStart = LuaNew.lua.GetLuaFunction("luaTabsPanelController.Start");
        tabsItemLuaStart.Call();
    }
	
    
}
