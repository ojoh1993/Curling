#define VER3

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 전반의 진행을 관리하는 클래스
/// </summary>
public class GameManager : MonoBehaviour {

    public GameObject Stone;
    public GameObject Obstacle;
    public GameObject Plane;
    public int totalScore, stoneRemains, timeRemains;

    public AudioSource InformSound;

    GameObject curObstacle;
    public List<GameObject> curStone = new List<GameObject>();

    private static GameManager gameManager;

    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!gameManager)
                {
                    Debug.LogError("There needs to be one active GameManager script on a GameObject in your scene.");
                }
                else
                {
                    gameManager.Init();
                }
            }

            return gameManager;
        }
    }

#if VER3
    private float friction;
    public float Friction
    {
        get
        {
            return friction;
        }
    }
#endif

    private void Init()
    {
        GameReset();
    }


    /// <summary>
    /// 게임 전체의 시작점.
    /// </summary>
    private void Start()
    {
        //게임 매니저 인스턴스를 1회 호출 하는 것으로 초기화..
        var g = instance;
    }


    /// <summary>
    /// 게임을 완전히 리셋하는 함수. 만약 기록할 시간 정보가 있다면 이 함수가 호출 됬을때 텍스트 파일에 기록한다.
    /// </summary>
    public void GameReset()
    {
        if(CamManager.instance != null)
        { 
            CamManager.instance.ResetCamPosition();
        }
        if (MarkerManager.instance != null)
        {
            MarkerManager.instance.ResetPosition();
        }
        if (curStone != null)
        {
            RemoveStones();
            if (curObstacle != null) Destroy(curObstacle);
            ReportManager.Report();
        }
        curObstacle = Instantiate(Obstacle);
        totalScore = 0;
        stoneRemains = 5;
        timeRemains = 15;

        UIManager.instance.SetGameResultText(totalScore, stoneRemains);
        UIManager.instance.SetTimeText(timeRemains);
        if(UIManager.instance.InformText != null)
            UIManager.instance.InformText.gameObject.SetActive(false);

        Skill.instance.SKILL_POWER_UP.skillInit();
        Skill.instance.SKILL_SCORE_DOUBLE.skillInit();
        UIManager.instance.skillButton1.GetComponent<ToggleButton>().init();
        UIManager.instance.skillButton2.GetComponent<ToggleButton>().init();

        ReloadStone();
    }

    public void RemoveStones()
    {
        foreach(var item in curStone)
        {
            Destroy(item);
        }
        curStone.Clear();
    }

    /// <summary>
    /// 돌을 재배치 하는 함수.
    /// </summary>
    public void ReloadStone()
    {
#if VER3
        friction = Random.value * 1f;
        UIManager.instance.SetDragText(Friction);
#endif        
        if(curStone.Count>0)
            curStone[curStone.Count - 1].GetComponent<StoneManager>().enabled = false;
        CamManager.instance.ResetCamPosition();
        if (stoneRemains == 0)
        {
            ReportManager.Report();
            return;
        }
        MarkerManager.instance.MoveToRandomSpace();        
        curStone.Add(Instantiate(Stone));
        timeRemains = 15;
        UIManager.instance.SetTimeText(timeRemains);
    }

    /// <summary>
    /// 판정된 점수를 받아서 기록하는 함수. 점수 판정은 Judge 클래스에서 내리고, 이 함수는 기록만 담당.
    /// </summary>
    /// <param name="score">기록할 점수 값</param>
    public void SetScore(int score)
    {
        stoneRemains--;
        totalScore += score;
        Debug.Log(totalScore);
        UIManager.instance.SetGameResultText(totalScore, stoneRemains);
    }

    /// <summary>
    /// 돌 발사 시간 제한을 위한 타이머 코루틴.
    /// 제 시간 안에 발사 되었을 경우 이 코루틴은 제거 되어야 함.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Timer()
    {
        while(timeRemains>0)
        {
            timeRemains--;
            UIManager.instance.SetTimeText(timeRemains);
            yield return new WaitForSeconds(1f);
        }

        SetScore(-1000);
        Destroy(curStone[curStone.Count - 1]);

        ReloadStone();
    }
}


