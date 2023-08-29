using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����1 : ���̽�ƽ�� ���ٰ� ������ ��� �ݴ� �������� ���� ���ư����� �Ѵ�.
//�Ӽ�1 : ���̽�ƽ, ���ư� ����, ���� ������ ���� ���� RigidBody, ������ ��
//�Ӽ��߰� : ���̽�ƽ�� ���� ���� �̺�Ʈ�� �߻��ϴµ� �׷��� ���̽�ƽ�� vertical, horizontal ���� 0�� �Ǳ⿡ ���̽�ƽ�� ���� ���� ���� �������� ���ư��� ���ؼ� tempH, tempV ���;
//����1-1. ���̽�ƽ�� ��� ���� �����Ѵ�.
//����1-2. ���̽�ƽ�� ���´�.
//����1-3. ���̽�ƽ�� ���� ���� ���� �������� ������ ���Ѵ�.
//����1-4. �ش� ������ ���� ������ ���� ���ؼ� ���� ���Ѵ�.


//����2 : ���̽�ƽ�� ��� �� ���ư��� ������ ǥ���ϴ� ���� �׸���.
//�Ӽ�2 : ���� �׸� LineRenderer
//����2-1. ���̽�ƽ�� ��� ������ ���� ���ư� ������ ���Ѵ�.
//����2-2. �ش� ������ LineRenderer�� �־��ش�.

//����3 : ���ӸŴ����� ���� ���� �����ϰ� �ڽ��� ���� �ƴ� ���� �۵����� �ʵ��� �Ѵ�.
//�Ӽ�3 : ���� ������ �� �ִ� ��
//����3-1. �� ���� �ƴϸ� return�Ѵ�.

public class BallMove : MonoBehaviour
{
    //�Ӽ�1 : ���̽�ƽ, ���ư� ����, ���� ������ ���� ���� RigidBody, ������ ��
    public VariableJoystick joystick;
    public Vector3 direction;
    protected new Rigidbody rigidbody;
    public float power = 380;
    //�Ӽ��߰� : ���̽�ƽ�� ���� ���� �̺�Ʈ�� �߻��ϴµ� �׷��� ���̽�ƽ�� vertical, horizontal ���� 0�� �Ǳ⿡ ���̽�ƽ�� ���� ���� ���� �������� ���ư��� ���ؼ� tempH, tempV ���;
    public float temph = 0;
    public float tempv = 0;

    //�Ӽ�2 : ���� �׸� LineRenderer
    protected LineRenderer lineRenderer;

    //�Ӽ�3 : ���� ������ �� �ִ� ��
    public int myTurn;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = GetComponent<MeshRenderer>().materials[0].GetColor("_Color");
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.2f;
        //Debug.Log(gameObject.name + "�� " + myTurn + "�� �����δ�.");
    }

    // Update is called once per frame
    void Update()
    {
        //����3-1. �� ���� �ƴϸ� return�Ѵ�.
        if (GameManager.Instance.turn != myTurn || !GameManager.Instance.isNobodyMove)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
            return;
        }


        //����1-1. ���̽�ƽ�� ��� ���� �����Ѵ�.
        float h = joystick.Horizontal;
        float v = joystick.Vertical;
        //���̽�ƽ�� ���� ���� h�� v���� 0�� �Ǳ⿡ �� ���� 0�� �ƴ� ��쿡�� temph, tempv�� �����Ѵ�
        if (h != 0 || v != 0)
        {
            temph = h;
            tempv = v;
        }

        //����1-2. ���̽�ƽ�� ���´�.     
        //����1-3. ���̽�ƽ�� ���� ���� ���� �������� ������ ���Ѵ�.
        //VariableJoyStick�� �����Ǿ� �ִ� OnPointerUp�� ���̽�ƽ�� ���� ������ �۵��ϱ⿡ �� �Լ��� ����� �� �ϴܿ� �ִ� Shoot()�Լ��� �����Ŵ���μ� ���� �߻��Ѵ�.
        direction = Vector3.left * temph + Vector3.back * tempv;

        //����2-1. ���̽�ƽ�� ��� ������ ���� ���ư� ������ ���Ѵ�.
        Vector3 offset = new Vector3(joystick.transform.position.x - joystick.Horizontal * 12, 0, joystick.transform.position.y - joystick.Vertical * 12);

        //����2-2. �ش� ������ LineRenderer�� �־��ش�.
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + offset);
    }

    public void Shoot()
    {
        //����1-4. �ش� ������ ���� ������ ���� ���ؼ� ���� ���Ѵ�.
        rigidbody.AddForce(direction * power, ForceMode.Impulse);
    }

    //2023/08/29 ���� OnColiisionEnter�� ������Ʈ�� �΋H�� ���� ���� ���� Console�� ǥ���ϱ� ���ؼ� ����ϹǷ� �ּ�ó����
    //private void OnCollisionEnter(Collision collision)
    //{
        
    //    Debug.Log(transform.position + "����" + collision.gameObject.name + "��" + rigidbody.velocity.magnitude + "�� �ӵ��� �΋H����.");
    //    if (collision.gameObject.layer != 3)
    //    {
    //        Debug.Log(gameObject.name + "��/�� " + collision.gameObject.name + "��/�� " + collision.contacts[0].normal + "�������� ����");
    //    }
    //}
}
