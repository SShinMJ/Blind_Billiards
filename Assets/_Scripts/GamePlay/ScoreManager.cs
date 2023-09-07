using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

//����1 : �� ���� ������ �����ϰ� �����Ѵ�.
//�Ӽ�1 : �÷��̾� �̸��� ������ ������ �ִ� Dictionary

public class ScoreManager : MonoBehaviour
{
    public Dictionary<string,int> ScoreList 
    {
        get
        {
            return _scoreList;
        }
        private set
        {
            _scoreList = value;
        }
    }
    //�÷��̾� �̸��� ������ ������ �ִ� Dict
    Dictionary<string, int> _scoreList = new();
    
    
}
