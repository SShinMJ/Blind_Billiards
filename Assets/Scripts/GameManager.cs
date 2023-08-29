using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//����1 : ���ӿ� ���� �����ؼ� �ڽ��� ���� �÷��̾ ������ �� �� �ֵ��� �Ѵ�.
//�Ӽ�1 : ���� ��, �����÷��̾� ������Ʈ ����
//�Ӽ��߰� : ���� �߻��� ���ĺ��� ���� ��� ���߰� ���� �Ѱ��ִ� ���̿��� ������ �� ������ �ϱ� ���ؼ� bool ������ isNobodyMove ����
//����1-1. ���� �÷��̾� ������Ʈ�� ��� ã�Ƽ� ���տ� �ִ´�.
//����1-2. ���տ� ������ ���� �� �÷��̾�� ���� �����Ѵ�.
//����1-3. ���̽�ƽ���� Shoot()�� ȣ������ �� �����̴� ���� ���ٸ� ���� �÷��̾ �ش��ϴ� ���� Shoot()�� �����Ѵ�.
//����1-4. Shoot()�� ������ ���Ŀ� ��� ���� ���߸� isNobodyMove���� ���� �������� �˷��ش�.

//����2 : ���ӿ� �ϰ� �� �Ͽ� �ش��ϴ� �÷��̾ UI�� ����Ѵ�.
//�Ӽ�2 : ���� �� UI, �� �÷��̾� UI
//����2-1. ���� ������ �� �� �Ͽ� �ش��ϴ� �÷��̾�� ���� ���� �־��ش�.
//����2-2. ���� �ٲ�� ���� ���� ���� �ٲ��ش�.

public class GameManager : MonoBehaviour
{
    //�̱����� �̿��ؼ� ���� ����� �� �ֵ��� ��
    public static GameManager Instance;

    //�Ӽ�1 : ���� ��, �����÷��̾� ������Ʈ ����
    public int turn;
    protected GameObject[] gamePlayers;

    //�Ӽ��߰� : ���� �߻��� ���ĺ��� ���� ��� ���߰� ���� �Ѱ��ִ� ���̿��� ������ �� ������ �ϱ� ���ؼ� bool ������ isNobodyMove ����
    public bool isNobodyMove = true;

    //�Ӽ�2 : ���� �� UI, �� �÷��̾� UI
    public TMP_Text currentTurn;
    public TMP_Text turnTable;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        turn = 0;
        //����1-1. ���� �÷��̾� ������Ʈ�� ��� ã�Ƽ� ���տ� �ִ´�.
        gamePlayers = GameObject.FindGameObjectsWithTag("Player");

        //����1-2. ���տ� ������ ���� �� �÷��̾�� ���� �����Ѵ�.
        for (int i = 0; i < gamePlayers.Length; i++)
        {
            gamePlayers[i].GetComponent<BallMove>().myTurn = i;
        }
    }

    private void Start()
    {
        //����2-1. ���� ������ �� �� �Ͽ� �ش��ϴ� �÷��̾�� ���� ���� �־��ش�.
        currentTurn.text = "Turn" + turn.ToString();
        turnTable.text = "Turn 0 : " + gamePlayers[0].name + "\n" + "Turn 1 : " + gamePlayers[1].name + "\n" + "Turn 2 : " + gamePlayers[2].name;
    }

    public void Shoot()
    {
        //����1-3. ���̽�ƽ���� Shoot()�� ȣ������ �� �����̴� ���� ���ٸ� ���� �÷��̾ �ش��ϴ� ���� Shoot()�� �����Ѵ�.
        if (isNobodyMove)
        {
            gamePlayers[turn].GetComponent<BallMove>().Shoot();

            //����1-4. Shoot()�� ������ ���Ŀ� ��� ���� ���߸� �ڷ�ƾ�� ���ؼ� isNobodyMove���� ���� �������� �˷��ش�.
            StartCoroutine(EndTurn());
        }
        return;
    }

    //����1-4. Shoot()�� ������ ���Ŀ� ��� ���� ���߸� isNobodyMove���� ���� �������� �˷��ش�.
    IEnumerator EndTurn()
    {
        //�� �ڷ�ƾ�� ���� �� �Ŀ� ����ǹǷ� isNobodyMove�� false�� �����Ѵ�.
        isNobodyMove = false;

        //��� ���� ���⶧ ���� �ݺ�
        while(!isNobodyMove)
        {
            yield return new WaitForSeconds(1f);
            //��� ���� �ӵ��� �����ؼ� 0.05���� �۴ٸ� isNobodyMove�� true���� �״�� �ְ� �ӵ��� 0.05���� ū ���� �ִٸ� isNobodyMove�� false�� �ְ� break�� ���� �ݺ����� ���´�.
            for (int i = 0; i < gamePlayers.Length; i++)
            {
                isNobodyMove = true;
                if (gamePlayers[i].GetComponent<Rigidbody>().velocity.magnitude > 0.05f)
                {
                    //Debug.Log(gamePlayers[i].gameObject.name + gamePlayers[i].GetComponent<Rigidbody>().velocity.magnitude);
                    isNobodyMove = false;
                    break;
                }
            }
        }

        //��� ���� �������� ���߰� �ݺ����� ������ ���Ŀ� ���� �����Ų��.
        if(turn < gamePlayers.Length-1)
        {
            turn++;
        }
        else
        {
            turn = 0;
        }

        //����2-2. ���� �ٲ�� ���� ���� ���� �ٲ��ش�.
        currentTurn.text = "Turn" + turn.ToString();
    }
}
