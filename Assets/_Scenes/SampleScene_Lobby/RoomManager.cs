using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

//����1 : �濡 �ִ� �÷��̾��� �̸��� ���ڸ� ǥ���Ѵ�.
//�Ӽ�1 : �ִ� �÷��̾� ��, �÷��̾� UI, �÷��̾� ����Ʈ

public class RoomManager : MonoBehaviour
{
    //�Ӽ�1 : �ִ� �÷��̾� ��, �÷��̾� UI, �÷��̾� ����Ʈ
    public int maxPlayerNum
    {
        get
        {
            return _maxPlayerNum;
        }
        set
        {
            if (value >= 3 && value <= 9)
            {
                _maxPlayerNum = value;
            }
        }
    }

    private int _maxPlayerNum = 3;
    public GameObject playerUI;
    [SerializeField] protected List<GameObject> playerList;
    public GameObject changeButton;

    public void AddPlayer()
    {
        if (maxPlayerNum < 9)
        {
            maxPlayerNum++;
            GameObject playerUIGO = Instantiate(playerUI, GameObject.Find("RoomUI").transform);
            playerUIGO.GetComponentInChildren<TextMeshProUGUI>().text = "----------";
            playerList.Add(playerUIGO);
            playerUIGO.transform.position += new Vector3(0, 320 -80 * (maxPlayerNum), 0);
            changeButton.transform.position += new Vector3(0, -80, 0);
        }
    }

    public void RemovePlayer()
    {
        if (maxPlayerNum > 3)
        {
            Destroy(playerList[playerList.Count - 1]);
            playerList.Remove(playerList[maxPlayerNum - 1]);
            maxPlayerNum--;
            changeButton.transform.position += new Vector3(0, 80, 0);
        }
    }
}
