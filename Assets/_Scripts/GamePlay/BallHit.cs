using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

//����1 : ���� �ٸ� ������Ʈ�� �浹�� ��� ���� �̵� ������ �����Ѵ�.
//�Ӽ�1 : ���� �̵� ����

public class BallHit : MonoBehaviour
{
    //�Ӽ�1 : ���� �̵� ����
    public MoveData moveData
    {
        get
        {
            if (true)
            {
                _moveData.startPos = transform.position;
            }
            _moveData.ballIndex = 1;
            _moveData.startTime = Time.time - GameManager.Instance.shootTime;

            return _moveData;
        }
    }
    private MoveData _moveData = new();

    // Start is called before the first frame update
    void Start()
    {
        _moveData.startPos = gameObject.transform.position;
    }

    //�浹�� �߻��� �� MoveDate����ü�� ���ӵ����Ϳ��� �״´�.
    private void OnCollisionEnter(Collision collision)
    {
        if (!GameManager.Instance.isNobodyMove)
        {
            if (!GuestReplayer.replaying)
            {
                GameManager.Instance.ballMoveData.Add(moveData);

                GetComponent<BallDoll>().CollisionEvent();
            }
        }

        if (collision.gameObject.tag is "Player")
        {
            if (GameManager.Instance.turn == 1)
            {
                //score++;

                GameManager.Instance.UpdateScore();
            }
        }
    }
}
