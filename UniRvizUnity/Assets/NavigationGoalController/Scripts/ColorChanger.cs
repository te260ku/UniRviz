using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class ColorChanger : MonoBehaviour
{
    //Dropdownを格納する変数
    [SerializeField] private Dropdown dropdown;
    //Cubeを格納する変数
    public GameObject cube;

    Color nowColor;
 
    // Update is called once per frame
    void Update()
    {
        //DropdownのValueが0のとき（赤が選択されているとき）
        if(dropdown.value == 0)
        {
            cube.GetComponent<Renderer>().material.color = Color.red;
            nowColor = Color.red;

        }
        //DropdownのValueが1のとき（青が選択されているとき）
        else if(dropdown.value == 1)
        {
            cube.GetComponent<Renderer>().material.color = Color.blue;
            nowColor = Color.blue;
        }
        //DropdownのValueが2のとき（黄が選択されているとき）
        else if (dropdown.value == 2)
        {
            cube.GetComponent<Renderer>().material.color = Color.yellow;
            nowColor = Color.yellow;
        }
        //DropdownのValueが3のとき（白が選択されているとき）
        else if (dropdown.value == 3)
        {
            cube.GetComponent<Renderer>().material.color = Color.white;
            nowColor = Color.white;
        }
        //DropdownのValueが4のとき（黒が選択されているとき）
        else if (dropdown.value == 4)
        {
            cube.GetComponent<Renderer>().material.color = Color.black;
            nowColor = Color.black;
        }
    }
}
