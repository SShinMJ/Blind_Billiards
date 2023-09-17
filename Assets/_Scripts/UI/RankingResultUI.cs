using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;

public class RankingResultUI : MonoBehaviour
{
    private static RankingResultUI instance;

    [SerializeField] GameObject[] rankObj;

    private int maxRankingRow = 5;

    List<RankData> savedRankDatas;

    [SerializeField]
    private Animator animator;

    private bool uiShow;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            uiShow = false;
            savedRankDatas = new List<RankData>();
        }
    }

    void Start()
    {
        savedRankDatas = new List<RankData>();
        maxRankingRow = rankObj.Length;
    }

    public void RankShow(List<RankData> rankDatas)
    {
        savedRankDatas.Clear();
        LoadRankData(rankDatas.Count);

        // �� ���� �߰�
        foreach (RankData rankData in rankDatas)
        {
            savedRankDatas.Add(rankData);
        }

        // score�� �������� ����
        savedRankDatas = savedRankDatas.OrderByDescending(x => x.score).ToList();

        // maxRankingRow ���� ������ ����
        if (maxRankingRow < savedRankDatas.Count)
        {
            savedRankDatas.RemoveRange(maxRankingRow, savedRankDatas.Count - maxRankingRow);
        }

        //Debug.LogWarning(savedRankDatas.Count + ", " + maxRankingRow);
        if (savedRankDatas.Count < maxRankingRow)
            maxRankingRow = savedRankDatas.Count;

        // ����� ���� ������ ����
        SaveRankData(rankDatas.Count);

        // ȭ�鿡 �����ֱ�.
        ShowRankData();

        if (TCP_BallCore.networkMode != NetworkMode.Server)
        {
            Invoke("ResetUI", 6f);
        }
    }

    // ������ �ο���(1p/2p/..)�� �ش��ϴ� ����� ��ŷ ������ �޾ƿ���
    void LoadRankData(int headCount)
    {
        RankData rankData = new RankData();
        maxRankingRow = rankObj.Length;
        for (int i = 0; i < maxRankingRow; i++)
        {
            // ���� key ���Ŀ��� : Rank 1P 1 playtime
            //         ����     : Rank | �÷��̾�� | ��ŷ(order) | playTime/score
            if (PlayerPrefs.HasKey("Rank " + headCount + "P " + i + "playtime"))
            {
                rankData.playTime = DateTime.Parse(PlayerPrefs.GetString("Rank " + headCount + "P " + i + "playtime"));
                rankData.id = PlayerPrefs.GetString("Rank " + headCount + "P " + i + "id");
                rankData.score = PlayerPrefs.GetInt("Rank " + headCount + "P " + i + "score");
                savedRankDatas.Add(rankData);
            }
        }
    }

    void ShowRankData()
    {
        int rankCount = 1;

        foreach (GameObject obj in rankObj)
        {
            // �ʿ���� �� ��Ȱ��ȭ
            if (rankCount > maxRankingRow)
            {
                obj.SetActive(false);
                continue;
            }

            obj.SetActive(true);

            // 0��° �ڽ� == ����
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (rankCount + 1).ToString();

            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = savedRankDatas[rankCount].id;

            // 1��° �ڽ� == ���� �ð�
            string date = savedRankDatas[rankCount].playTime.ToString("yyyy-MM-dd") +
                "\n" + savedRankDatas[rankCount].playTime.ToString("HH:mm");
            obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = date;

            // 2��° �ڽ� == ����
            obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = savedRankDatas[rankCount].score.ToString();

            rankCount++;
        }
        animator.Play("In");
    }

    // savedRankDatas PlayerPrefs ����
    void SaveRankData(int headCount)
    {
        maxRankingRow = rankObj.Length;
        for (int i = 0; i < maxRankingRow; i++)
        {
            if (savedRankDatas.Count <= i)
            {
                Debug.LogWarning("out!");
                break;
            }
            PlayerPrefs.SetString("Rank " + headCount + "P " + i + "playTime", savedRankDatas[i].playTime.ToString());
            PlayerPrefs.SetString("Rank " + headCount + "P " + i + "id", savedRankDatas[i].id);
            PlayerPrefs.SetString("Rank " + headCount + "P " + i + "score", savedRankDatas[i].score.ToString());
        }
        //Debug.LogWarning(savedRankDatas.Count + ", !" + maxRankingRow);
    }

    public static void StartRankUI(List<RankData> rankDatas)
    {
        if (!instance.uiShow)
        {
            instance.uiShow = true;
            instance.RankShow(rankDatas);
        }
    }

    private void ResetUI()
    {
        animator.Play("Out");
        uiShow = false;
    }
    //public void UpdateRankData(List<RankData> rankDatas)
    //{
    //    RankShow(rankDatas);
    //}
}

public struct RankData
{
    public string id;
    public DateTime playTime;
    public int score;
}