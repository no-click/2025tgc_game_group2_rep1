using UnityEngine;
using System.Collections.Generic;

public class HPicon1 : MonoBehaviour
{
    public GameObject hpicon;
    private Player player;
    private int beforeHP;
    private List<GameObject> hpiconList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        beforeHP = player.GetHP();
        CreateHPicon();
    }

    public void CreateHPicon()
    {
        for (int i = 0; i < player.GetHP(); i++)
        {
            GameObject icon = Instantiate(hpicon);
            icon.transform.SetParent(transform);
            hpiconList.Add(icon);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        ShowHPicon();
    }

    void ShowHPicon()
    {
        if (beforeHP == player.GetHP()) return;
        for (int i = 0; i < hpiconList.Count; i++)
        {
            hpiconList[i].SetActive(i < player.GetHP());

        }
        beforeHP = player.GetHP();


    }
}
    
