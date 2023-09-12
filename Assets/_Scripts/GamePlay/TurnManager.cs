using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//����1 : ���� ���� ������ �ְ� ���� ����Ǹ� ���� �ѱ��.
//�Ӽ�1 : ���� ��
//����1 : ���� ���� ������ �ѱ��.

//����2 : �Ͽ� �ش��ϴ� ���� ��ȯ�Ѵ�.
//�Ӽ�2 : �÷��̾� �� ����Ʈ
//����2 : �Ͽ� �ش��ϴ� ���� ��ȯ�Ѵ�.

//�ַ��÷��� ������ �� ���� ����Ǹ� ���� �� ����� MyPlayer�� �����Ѵ�.

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    //�Ӽ�1 : ���� ��
    public int currentTurn
    {
        get
        {
            return _currentTurn;
        }
        private set
        {
            _currentTurn = value;
        }
    }
    int _currentTurn;

    //�Ӽ�2 : �÷��̾� �� ����Ʈ
    [SerializeField] List<GameObject> ballList;

    //����1 : ���� ���� ������ �ѱ��.
    public void EndTurn()
    {
        foreach(var data in GameManager.Instance.entryPlayerDataList)
        {
            if(data.id == GetTurnBall().name)
            {
                TCP_BallServer.TurnEnd(data.score);
                break;
            }
        }

        currentTurn++;
        if(currentTurn >= GameManager.Instance.gamePlayers.Count)
        {
            currentTurn = 0;
        }
        GameManager.Instance.joystick.GetComponent<BallLineRender>().ResetBallStatus();
        UIManager.Instance.UpdateTurn(currentTurn);
        if(TCP_BallCore.networkMode == NetworkMode.None)
        {
            GameManager.Instance.SoloPlaySet(currentTurn);
        }

        //GuestReplayer.ReplayTurn(GameManager.Instance.ballMoveData);

        //if(GetTurnBall().name != GameManager.Instance.myID)
        //{
        //    GameManager.Instance.joystick.gameObject.SetActive(false);
        //}
        //else
        //{
        //    GameManager.Instance.joystick.gameObject.SetActive(true);
        //}
    }

    public void EndTurn(int _countOfMoveData, int _differenceOfScore)
    {

        if(_countOfMoveData != GameManager.Instance.ballMoveData.Count)
        {
            Debug.LogError("�� �����Ϳ� ������ �߻��߽��ϴ�.");
            return;
        }

        for (int i = 0; i < _differenceOfScore; i++)
        {
            ScoreManager.Instance.PlusScore(GetTurnBall());
        }

        currentTurn++;
        if(currentTurn >= GameManager.Instance.gamePlayers.Count)
        {
            currentTurn = 0;
        }

        GameManager.Instance.joystick.GetComponent<BallLineRender>().ResetBallStatus();
        UIManager.Instance.UpdateTurn(currentTurn);
        if (TCP_BallCore.networkMode == NetworkMode.None)
        {
            GameManager.Instance.SoloPlaySet(currentTurn);
        }

        GuestReplayer.ReplayTurn(GameManager.Instance.ballMoveData);
    }

    //����2 : �Ͽ� �ش��ϴ� ���� ��ȯ�Ѵ�.
    public GameObject GetTurnBall()
    {
        if(ballList == null)
        {
            return null;
        }
        if(currentTurn >= ballList.Count)
        {
            int tempTurn = currentTurn;
            while(tempTurn >= ballList.Count)
            {
                tempTurn -= ballList.Count;
            }
            return ballList[tempTurn];
        }
        return ballList[currentTurn];
    }

    public GameObject GetTurnBall(int turn)
    {
        if(ballList == null)
        {
            return null;
        }
        return ballList[turn];
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GetListFromGameManager()
    {
        ballList = GameManager.Instance.gamePlayers;

        //if (GetTurnBall().name != GameManager.Instance.myID)
        //{
        //    GameManager.Instance.joystick.gameObject.SetActive(false);
        //}
        //else
        //{
        //    GameManager.Instance.joystick.gameObject.SetActive(true);
        //}
    }
}
