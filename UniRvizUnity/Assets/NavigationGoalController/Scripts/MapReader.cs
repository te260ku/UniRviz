using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// マップをインポートするスクリプト
/// </summary>
public class MapReader : MonoBehaviour
{
    [SerializeField] string _imagePath;
    [SerializeField] string _yamlPath;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] GameObject _plane;
    [SerializeField] GameObject _mapOriginObj;
    [SerializeField] GameObject _tb3Obj;
    [NonSerialized] public Vector3 _originPos;

    public void ImportMap() {
        ReadMap(_imagePath);
    }

    public void ReadMap(string path) {
        var texture = TextureReader.GetTextureFromPngFile(path);
        meshRenderer.material.mainTexture = texture;
        
        var yaml = YamlReader.Deserialize(_yamlPath);
        var scaleX = yaml.resolution * texture.width * 0.1f;
        var scaleZ = yaml.resolution * texture.height * 0.1f;
        _plane.transform.localScale = new Vector3(scaleX, 1f, scaleZ);
        var posX = yaml.resolution * texture.width * 0.1f * yaml.origin[0];
        var posZ = yaml.resolution * (texture.width/2 - texture.height/2) * -1f;
        
        _plane.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        _plane.transform.position = new Vector3(0f, 0f, 0f);
        
        _mapOriginObj.transform.position = new Vector3(-texture.height/2f*0.05f, 0f, -texture.width/2f*0.05f);
        
        _tb3Obj.transform.position = new Vector3(
            ((-texture.height/2f*0.05f)+(yaml.origin[1]*-1f))*-1f, 
            0f, 
            (-texture.width/2f*0.05f)+(yaml.origin[0]*-1f)
        );

        _originPos = new Vector3(
            ((-texture.height/2f*0.05f)+(yaml.origin[1]*-1f))*-1f, 
            0f, 
            (-texture.width/2f*0.05f)+(yaml.origin[0]*-1f)
        );
    }

}
