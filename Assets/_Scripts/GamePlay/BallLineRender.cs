using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����1 : ���̽�ƽ�� ��� �� ���ư��� ������ ǥ���ϴ� ���� �׸���.
//�Ӽ�1 : �Է°��� �޾ƿ� JoyStick, ���� �׸� LineRenderer, ���� ã�� Ray�� RayCastHit, ���̽�ƽ�� ���������� Ȯ���� isClicked, ���� ��ġ
//����1-1. ���̽�ƽ�� ����.
//����1-2. �ش� �������� Ray�� ������.
//����1-3. Ray�� ���� �΋H���� �� �Ÿ��� LineRenderer���� �ش�.

public class BallLineRender : MonoBehaviour
{
    //�Ӽ�1 : �Է°��� �޾ƿ� JoyStick, ���� �׸� LineRenderer, ���� ã�� Ray�� RayCastHit, ���̽�ƽ�� ���������� Ȯ���� isClicked, ���� ��ġ
    public bool isClicked = false;

    FixedJoystick joystick;
    LineRenderer lineRenderer;
    Ray lineRay;
    RaycastHit hitinfo = new();
    Vector3 ballPosition;

    // Start is called before the first frame update
    void Start()
    {
        joystick = GetComponent<FixedJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isNobodyMove)
        {
            if (isClicked)
            {
                if (GameManager.Instance.CheckMyBall() || TCP_BallCore.networkMode == NetworkMode.None)
                {
                    Vector3 offset = new Vector3(- joystick.Horizontal, 0, - joystick.Vertical);
                    lineRay = new Ray(ballPosition, offset.normalized);
                    int _layerMask = 1 << LayerMask.NameToLayer("Wall");
                    if (Physics.Raycast(lineRay, out hitinfo, 10, _layerMask))
                    {
                        //����2-2. �ش� ������ LineRenderer�� �־��ش�.
                        lineRenderer.SetPosition(0, ballPosition);
                        lineRenderer.SetPosition(1, ballPosition + offset * hitinfo.distance);
                    }
                    else
                    {
                        //����2-2. �ش� ������ LineRenderer�� �־��ش�.
                        lineRenderer.SetPosition(0, ballPosition);
                        lineRenderer.SetPosition(1, ballPosition + offset * 2);
                    }
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(0, ballPosition);
            lineRenderer.SetPosition(1, ballPosition);
        }
    }

    public void ResetBallStatus()
    {
        lineRenderer = TurnManager.Instance.GetTurnBall().GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.white;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.endColor = TurnManager.Instance.GetTurnBall().GetComponent<BallDoll>().showcaseColor;
        ballPosition = TurnManager.Instance.GetTurnBall().transform.position;
    }
}
