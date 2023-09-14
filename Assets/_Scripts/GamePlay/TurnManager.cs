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
        currentTurn++;
        if(currentTurn >= GameManager.Instance.gamePlayers.Count)
        {
            currentTurn = 0;
        }

        Debug.Log("CurrentTurn: " + currentTurn);
        GameManager.Instance.InitSetting();
        GameManager.Instance.ClearMoveData();
    }

    public void EndTurn(int _countOfMoveData, int _differenceOfScore)
    {
        Debug.LogError("Call TurnEnd");
        if(TCP_BallCore.networkMode == NetworkMode.Server)
        {
            EndTurn();
            return;
        }
        else
        {
            Debug.LogError(_countOfMoveData);
            Debug.LogError(GameManager.Instance.ballMoveData.Count);


            if (_countOfMoveData != GameManager.Instance.ballMoveData.Count)
            {
                Debug.LogError("�� �����Ϳ� ������ �߻��߽��ϴ�.");
                return;
            }

            for (int i = 0; i < _differenceOfScore; i++)
            {
                ScoreManager.Instance.PlusScore(GetTurnBall());
            }

            GuestReplayer.ReplayTurn(GameManager.Instance.ballMoveData);

            currentTurn++;
            if (currentTurn >= GameManager.Instance.gamePlayers.Count)
            {
                currentTurn = 0;
            }

            Debug.Log("CurrentTurn: " + currentTurn);
            GameManager.Instance.InitSetting();
        }
    }

    //����2 : �Ͽ� �ش��ϴ� ���� ��ȯ�Ѵ�.
    public GameObject GetTurnBall()
    {
        if(ballList == null)
        {
            Debug.LogError("BallList is Empty!");
            return null;
        }
        return ballList[currentTurn];
    }

    public GameObject GetTurnBall(int turn)
    {
        if(ballList == null)
        {
            Debug.LogError("BallList is Empty!");
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
    }
}
