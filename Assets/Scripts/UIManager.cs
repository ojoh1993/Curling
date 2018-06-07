using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 게임 UI전반을 관리하는 클래스
/// </summary>
public class UIManager : MonoBehaviour {

    public GameObject UI_Arrow;
    public Vector3 mouseClickedPosition;
    public Text scoreText, stoneText, timeText, FrictionText, InformText;
    public Button resetButton, skillButton1, skillButton2;
    public SkillContainer skill1, skill2;

    private static UIManager uiManager;

    public static UIManager instance
    {
        get
        {
            if (!uiManager)
            {
                uiManager = FindObjectOfType(typeof(UIManager)) as UIManager;

                if (!uiManager)
                {
                    Debug.LogError("There needs to be one active UIManager script on a GameObject in your scene.");
                }
                else
                {
                    uiManager.Init();
                }
            }

            return uiManager;
        }
    }

    private void Init()
    {
        resetButton.onClick.AddListener(OnResetButtonClick);
        skillButton1.onClick.AddListener(OnSkill1buttonClick);
        skillButton2.onClick.AddListener(OnSkill2buttonClick);
    }

    /// <summary>
    /// 남은 시간을 화면에 표시하는 함수.
    /// </summary>
    /// <param name="timeRemain"></param>
    public void SetTimeText(int timeRemain)
    {
        timeText.text = "" + timeRemain;
    }

    /// <summary>
    /// 남은 시간을 화면에 표시하는 함수.
    /// </summary>
    /// <param name="timeRemain"></param>
    public void SetDragText(float drag)
    {
        var d = drag.ToString();
        if (d.Length >= 5) d = d.Substring(0, 5);
        FrictionText.text = "FRC : " + d;
    }

    /// <summary>
    /// 남은 시간을 화면에 표시하는 함수.
    /// </summary>
    /// <param name="timeRemain"></param>
    public void SetVelocityText(float vel)
    {
        var d = vel.ToString();
        if (d.Length >= 5) d = d.Substring(0, 5);
        FrictionText.text = "SPD : " + d;
    }

    /// <summary>
    /// 점수와 남은 돌 갯수를 화면에 표시하는 함수.
    /// </summary>
    /// <param name="totalScore">현재 총 점수</param>
    /// <param name="stoneRemains">현재 남은 돌의 갯수</param>
    public void SetGameResultText(int totalScore, int stoneRemains)
    {
        scoreText.text = "Score : " + totalScore;
        stoneText.text = "Remains : " + stoneRemains;
    }

    /// <summary>
    /// 리셋 버튼이 클릭 되었을때 불려오는 함수.
    /// </summary>
    public void OnResetButtonClick()
    {
        GameManager.instance.GameReset();
    }

    public void OnSkill1buttonClick()
    {
        if (!skill1.skillUsed)
        {
            skill1.triggered = !skill1.triggered;
        }
    }

    public void OnSkill2buttonClick()
    {
        if (!skill2.skillUsed)
        {
            skill2.triggered = !skill2.triggered;
        }
    }

}
