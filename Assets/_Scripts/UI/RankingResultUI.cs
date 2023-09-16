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
    public struct RankData
    {
        public string id;
        public DateTime playTime;
        public int score;
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }

    void Start()
    {
        savedRankDatas = new List<RankData>();
        maxRankingRow = rankObj.Length;
    }

    public void RankShow(List<RankData> rankDatas)
    {
        LoadRankData(rankDatas.Count);
        maxRankingRow = rankDatas.Count;

        // �� ���� �߰�
        foreach (RankData rankData in rankDatas)
        {
            savedRankDatas.Add(rankData);
        }

        // score�� �������� ����
        savedRankDatas = savedRankDatas.OrderByDescending(x => x.score).ToList();

        // maxRankingRow ���� ������ ����
        if(maxRankingRow < savedRankDatas.Count)
        {
            savedRankDatas.RemoveRange(maxRankingRow, savedRankDatas.Count-maxRankingRow);
        }

        if (savedRankDatas.Count < maxRankingRow)
            maxRankingRow = savedRankDatas.Count;

        // ȭ�鿡 �����ֱ�.
        ShowRankData();

        // ����� ���� ������ ����
        SaveRankData(rankDatas.Count);

        Invoke("ResetUI", 6f);
    }

    // ������ �ο���(1p/2p/..)�� �ش��ϴ� ����� ��ŷ ������ �޾ƿ���
    void LoadRankData(int headCount)
    {
        RankData rankData = new RankData();
        for (int i = 1; i <= maxRankingRow; i++)
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
                break;

            obj.SetActive(true);

            // 0��° �ڽ� == ����
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rankCount.ToString();

            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = savedRankDatas[rankCount - 1].id;

            // 1��° �ڽ� == ���� �ð�
            string date = savedRankDatas[rankCount - 1].playTime.ToString("yyyy-MM-dd") +
                "\n" + savedRankDatas[rankCount - 1].playTime.ToString("HH:mm");
            obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = date;

            // 2��° �ڽ� == ����
            obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = savedRankDatas[rankCount - 1].score.ToString();

            rankCount++;
        }
    }

    // savedRankDatas PlayerPrefs ����
    void SaveRankData(int headCount)
    {
        for (int i = 1; i <= maxRankingRow; i++)
        {
            PlayerPrefs.SetString("Rank " + headCount + "P " + i + "playTime", savedRankDatas[i - 1].playTime.ToString());
            PlayerPrefs.SetString("Rank " + headCount + "P " + i + "id", savedRankDatas[i - 1].id);
            PlayerPrefs.SetString("Rank " + headCount + "P " + i + "score", savedRankDatas[i - 1].score.ToString());
        }
    }

    public static void StartRankUI(List<RankData> rankDatas)
    {
        instance.RankShow(rankDatas);
    }

    private void ResetUI()
    {
        foreach (GameObject obj in rankObj)
        {
            obj.SetActive(true);
        }
    }
}
