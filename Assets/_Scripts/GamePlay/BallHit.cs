using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

//����1 : ������ �ַλ��¶�� ���� �ٸ� ������Ʈ�� �浹�� ��� ���� �̵� ������ �����Ѵ�.
//�Ӽ�1 : ���� �̵� ����

public class BallHit : MonoBehaviour
{
    //�Ӽ�1 : ���� �̵� ����
    public MoveData moveData
    {
        get
        {
            if (GetComponent<BallMove>().isMove)
            {
                _moveData.startPos = transform.position;
            }
            _moveData.ballIndex = GameManager.Instance.GetIndexOfBall(name);
            if(TCP_BallCore.networkMode == NetworkMode.Client)
            {
                _moveData.startTime = Time.time - GameManager.Instance.shootTime;
                Debug.LogError("Client Time is : " + Time.time + "  And ShootTime is " + GameManager.Instance.shootTime);
            }
            else
            {
                _moveData.startTime = Time.time - GameManager.Instance.shootTime;
                Debug.LogError("Server Time is : " + Time.time + "  And ShootTime is " + GameManager.Instance.shootTime);
            }


            return _moveData;
        }
    }

    private MoveData _moveData;

    void Start()
    {
        _moveData = new MoveData();
        _moveData.startPos = gameObject.transform.position;

        if (GetComponent<BallMove>().isMove)
        {
            _moveData.startPos = transform.position;
        }
        _moveData.ballIndex = GameManager.Instance.GetIndexOfBall(this.name);
        _moveData.startTime = Time.time - GameManager.Instance.shootTime;
    }

    //�浹�� �߻��� �� MoveDate����ü�� ���ӵ����Ϳ��� �״´�.
    private void OnCollisionEnter(Collision collision)
    {
        if (!GameManager.Instance.isNobodyMove && !GuestReplayer.replaying)
        {
            if (TCP_BallCore.networkMode == NetworkMode.Server)
            {
                TCP_BallServer.Moved(moveData);
                GameManager.Instance.AddMoveData(moveData);
            }
            if(TCP_BallCore.networkMode != NetworkMode.Client)
            {
                GetComponent<BallDoll>().CollisionEvent();

                if(collision.gameObject.tag is "Player")
                {
                    if(TurnManager.Instance.GetTurnBall() == this.gameObject)
                    {
                        ScoreManager.Instance.PlusScore(this.gameObject);
                    }
                }
            }
        }
    }
}
