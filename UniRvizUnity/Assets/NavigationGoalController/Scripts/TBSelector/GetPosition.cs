using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �}�E�X�J�[�\���̍��W���擾����X�N���v�g
/// </summary>
public class GetPosition : MonoBehaviour
{
    [SerializeField] Text _cursorPositionText;
    Vector3 _cursorPosition;
    Vector3 _cursorPosition3d;
    RaycastHit hit;
    public Vector3 _clickPosition;
    
    
    void Start()
    {
        _clickPosition = new Vector3(0f, -2f, 0f);
        _cursorPositionText.text = "";
    }


    void Update()
    {
        _cursorPosition = Input.mousePosition; // ��ʏ�̃J�[�\���̈ʒu
        _cursorPosition.z = 10.0f; // z���W�ɓK���Ȓl������
        _cursorPosition3d = Camera.main.ScreenToWorldPoint(_cursorPosition); // �O�������W�ɕϊ�����

        if (Input.GetMouseButton(0))
        {
            transform.position = _clickPosition;
        } else {
            if (Physics.Raycast(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position) * hit.distance, Color.red);

                _cursorPositionText.text = "Mouse"+"\n" + hit.point.ToString();
                transform.position = hit.point;
            }
        }
        
        
        
       
    }

    
}