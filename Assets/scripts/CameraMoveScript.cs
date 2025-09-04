using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    //プレイヤーのオブジェクト
    public GameObject playerObj;
    public float PosX = 5;
    public float PosY = 0;

    void Update()
    {
        //カメラをプレイヤーに追従させる
        this.transform.position = new Vector3(playerObj.transform.position.x + PosX, this.transform.position.y + PosY, this.transform.position.z);
    }
}

