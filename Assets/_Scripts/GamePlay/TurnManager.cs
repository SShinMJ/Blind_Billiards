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
    GameObject[] ballList;

    //����1 : ���� ���� ������ �ѱ��.
    public void EndTurn()
    {
        currentTurn++;
        GameObject.Find("VariableJoystick").GetComponent<BallLineRender>().ResetBallStatus();
    }

    //����2 : �Ͽ� �ش��ϴ� ���� ��ȯ�Ѵ�.
    public GameObject GetTurnBall()
    {
        return ballList[currentTurn];
    }
    public GameObject GetTurnBall(int turn)
    {
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
}
