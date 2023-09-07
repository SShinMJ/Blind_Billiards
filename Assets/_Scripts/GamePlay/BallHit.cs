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
            _moveData.ballIndex = ballID;
            _moveData.startTime = Time.time - GameManager.Instance.shootTime;

            return _moveData;
        }
    }
    private MoveData _moveData = new();

    public int ballID;

    void Start()
    {
        _moveData.startPos = gameObject.transform.position;
    }

    //�浹�� �߻��� �� MoveDate����ü�� ���ӵ����Ϳ��� �״´�.
    private void OnCollisionEnter(Collision collision)
    {
        if(TCP_BallCore.networkMode != NetworkMode.Client)
        {
            if (!GameManager.Instance.isNobodyMove)
            {
                if (!GuestReplayer.replaying)
                {
                    GameManager.Instance.ballMoveData.Add(moveData);

                    GetComponent<BallDoll>().CollisionEvent();
                }
            }
        }
    }
}
