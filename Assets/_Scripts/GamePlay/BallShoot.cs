using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����1 : ���̽�ƽ�� �Է¿� ���� ���ϴ� �������� ���ϴ� ������ ���� �߻��Ѵ�.
//�Ӽ�1 : ���̽�ƽ, h, v, tempV, tempH, Power, ����

public class BallShoot : MonoBehaviour
{
    //�Ӽ�1 : ���̽�ƽ, h, v, tempV, tempH, Power, ����
    [SerializeField] float power = 50;

    VariableJoystick joystick;
    float tempV = 0;
    float tempH = 0;

    Vector3 direction;

    private void Start()
    {
        joystick = GetComponent<VariableJoystick>();
    }

    private void Update()
    {
        float h = joystick.Horizontal;
        float v = joystick.Vertical;

        if (h != 0 || v != 0)
        {
            tempH = h;
            tempV = v;
        }

        direction = Vector3.left * tempH + Vector3.back * tempV;
    }

    public void Shoot()
    {
       TurnManager.Instance.GetTurnBall().GetComponent<Rigidbody>().AddForce(direction * power, ForceMode.Impulse);
    }
}
