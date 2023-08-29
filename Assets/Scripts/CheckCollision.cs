using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    protected new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag is "Player")
        {
            Debug.Log("���� ���� " + rigidbody.velocity.magnitude + "�� �ӵ��� " + rigidbody.velocity.normalized + "�������� �̵���");
        }
    }
}
