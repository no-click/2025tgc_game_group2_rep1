using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    //�v���C���[�̃I�u�W�F�N�g
    public GameObject playerObj;
    public float PosX = 5;
    public float PosY = 0;

    void Update()
    {
        //�J�������v���C���[�ɒǏ]������
        this.transform.position = new Vector3(playerObj.transform.position.x + PosX, this.transform.position.y + PosY, this.transform.position.z);
    }
}

