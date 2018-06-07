#define VER3
#define VER2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 돌에 관한 사항들을 관리하는 클래스.
/// 발사에 필요한 힘/UI(화살표), 발사 까지 걸리는 시간, 발사 이후 돌의 재정비등을 담당
/// </summary>
public class StoneManager : MonoBehaviour {
    
    const float MAX_POWER = 4f;
    const float BOOST_SCALE = 3f;
    const float IMPULSE_SCALE = 1f;
    
    // 돌의 현재 상태를 나타냄. 최초 생성 / 준비 / 발사 후 움직이는 중 / 발사 후 정지 
    public enum state { init, ready, running, stop };
    public state cur_state = state.init;

    public AudioSource Disturb;
    public AudioSource Bump;
    public Judge judge;

    public SkillContainer skill_script_power_up;
    public SkillContainer skill_script_score_double;

    /*/*/  /*/*/

    private float skill_power_effect = 1f;
    private int skill_score_effect = 1;
    private bool isClicked = false;
    private bool impulsed = false;

    private GameObject arrow;
    private Coroutine Timerhandle;

    private float interval;

    private struct power_info
    {
        public float angle, scale;
        public power_info(float a, float s)
        {
            angle = a;
            scale = s;
        }
    }
    private power_info pow;




    private void OnEnable()
    {
#if VER3
        GetComponent<Collider>().material.dynamicFriction = GameManager.instance.Friction;
#endif
        skill_script_power_up = Skill.instance.SKILL_POWER_UP;
        skill_script_power_up.SkillRegister(Skill_power_up);
        skill_script_score_double = Skill.instance.SKILL_SCORE_DOUBLE;
        skill_script_score_double.SkillRegister(Skill_score_double);
#if VER4
        Calibration.instance.SetCalibration();
#endif
    }



    // Update is called once per frame
    void Update () {
        
        // 돌이 생성되고 바닥에 떨어져서 완전히 정지 되면, 준비 상태로 들어감.
        if (cur_state == state.init && 
            judge.ready &&
            GetComponent<Rigidbody>().velocity.magnitude == 0f)
        {
            cur_state = state.ready;
        }

        switch (cur_state)
        {
            case state.ready:
                whenReady();
                break;

            case state.running:
                whenRunning();
                break;

            case state.stop:
                whenStop();
                break;
        }
	}

    public void whenReady()
    {
        // 발사 이후 돌에 속력이 일정 이상 붙으면, 움직이는 중으로 상태 전환.
        if (GetComponent<Rigidbody>().velocity.magnitude >= 0.1f)
            cur_state = state.running;

        // 발사시간 제한을 위한 타이머를 세팅한다. 
        if (Timerhandle == null)
            Timerhandle = StartCoroutine(GameManager.instance.Timer());

        // 발사 UI(화살표)가 생긴 후에, 마우스 위치에 따라 힘과 방향을 조절 하는 부분.
        if (arrow != null)
        {
            var Currentpos = Input.mousePosition;
            var diff = Currentpos - UIManager.instance.mouseClickedPosition;

            pow.angle = -Mathf.Atan2(diff.y, diff.x);
            pow.scale = Mathf.Min(Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y) / 50, MAX_POWER);

            arrow.transform.rotation = Quaternion.Euler(90f, pow.angle * Mathf.Rad2Deg - 90f, 0f);
            arrow.transform.localScale = new Vector3(.3f, pow.scale / 1.5f);
        }
    }

    public void whenRunning()
    {
        if (Timerhandle != null)
        {
            // 타이머가 동작 중 이었다면, 성공적으로 발사 되었으므로 타이머를 제거한다.
            StopCoroutine(Timerhandle);
            Timerhandle = null;
        }


        // 카메라를 돌에 붙임
        CamManager.instance.AttachCamToStone(gameObject);

        UIManager.instance.SetVelocityText(GetComponent<Rigidbody>().velocity.magnitude);

        // UI(화살표)가 생긴 후에, 마우스 위치에 따라 힘과 방향을 조절 하는 부분.
        if (arrow != null)
        {
            var Currentpos = Input.mousePosition;
            var diff = Currentpos - UIManager.instance.mouseClickedPosition;

            pow.angle = -Mathf.Atan2(diff.y, diff.x);
            if (pow.angle >= 90f * Mathf.Deg2Rad)
                pow.angle -= 270f * Mathf.Deg2Rad;
            else
                pow.angle += 90f * Mathf.Deg2Rad;
            pow.angle = Mathf.Clamp(pow.angle, -90f * Mathf.Deg2Rad, 90f * Mathf.Deg2Rad) - 90f * Mathf.Deg2Rad;

            arrow.transform.rotation = Quaternion.Euler(90f, pow.angle * Mathf.Rad2Deg + 90f, 0f);
            var diffvel = new Vector3(Mathf.Cos(pow.angle), 0f, -Mathf.Sin(pow.angle)) * Time.deltaTime * .25f;
            GetComponent<Rigidbody>().AddForce(diffvel);
#if VER3
            if (Input.GetKeyDown("space") && !impulsed)
            {
                impulsed = true;
                var impulse = new Vector3(Mathf.Cos(pow.angle), 0f, -Mathf.Sin(pow.angle)) * IMPULSE_SCALE;
                GetComponent<Rigidbody>().AddForce(impulse);

                UIManager.instance.InformText.gameObject.SetActive(false);
            }
#endif
        }
        // 돌의 속력이 0이 되면 멈춘 상태로 전환
        if (GetComponent<Rigidbody>().velocity.magnitude == 0f)
            cur_state = state.stop;
    }

    public void whenStop()
    {
#if VER3
        skill_script_power_up.triggered = false;
        UIManager.instance.InformText.gameObject.SetActive(false);
#endif
        // 점수 판정을 내리고
        skill_script_score_double.SkillTrigger();
        GameManager.instance.SetScore(judge.score * skill_score_effect);
        // 돌을 재배치
        GameManager.instance.ReloadStone();
        Destroy(judge);
        Destroy(this);

    }

    // 돌을 클릭하면 반응 하는 함수
    public void OnMouseDown()
    {
        if (cur_state == state.ready)
        {
            // 화살표 UI를 생성하고, 마우스 원점을 기록하여 마우스 움직임에 따라 힘을 조절 할수 있게 한다.
            arrow = Instantiate(UIManager.instance.UI_Arrow);
            arrow.transform.parent = gameObject.transform;
            arrow.transform.position = this.transform.position + new Vector3(0, 4, 0);
            arrow.transform.localScale = new Vector3(.3f, .3f, .3f);
            UIManager.instance.mouseClickedPosition = Input.mousePosition;
            isClicked = true;
            interval = Time.time;

        }
        else if (cur_state == state.running)
        {
            // 화살표 UI를 생성하고, 마우스 원점을 기록하여 마우스 움직임에 따라 힘을 조절 할수 있게 한다.
            arrow = Instantiate(UIManager.instance.UI_Arrow);
            arrow.transform.parent = gameObject.transform;
            arrow.transform.position = this.transform.position + new Vector3(0, 4, 0);
            arrow.transform.localScale = new Vector3(.3f, .3f, .3f);
            UIManager.instance.mouseClickedPosition = Input.mousePosition;
            isClicked = true;
            interval = Time.time;
        }
    }

    // 돌을 클릭하고 마우스를 뗐을 때 반응 하는 함수
    public void OnMouseUp()
    {
        if (cur_state == state.ready && isClicked)
        {
            // 화살표 UI를 없애고, 마우스 움직임에 따라 정해진 힘을 돌에 가한다. 그리고 클릭 이후 마우스를 떼기까지 걸린 시간을 기록한다.
            Destroy(arrow);
#if VER4
            var mistake = Calibration.instance.GetCalibrationRatio();
            if(Mathf.Abs(mistake) > .3f)
                AudioSourceManager.instance.Play("inform_sound", GameManager.instance.InformSound);
            pow.angle += Calibration.CALIBRATION_ERROR_MAX * mistake;
            Calibration.instance.UnsetCalibration();
#endif
#if VER3
            if(UIManager.instance.InformText != null)
                UIManager.instance.InformText.gameObject.SetActive(true);
#endif
#if VER2
            if(Random.value < .2f)
            {
                AudioSourceManager.instance.Play("inform_sound",GameManager.instance.InformSound);
                pow.angle += Calibration.CALIBRATION_ERROR_MAX * (Random.value * .4f - .2f);
            }
#endif
            skill_script_power_up.SkillTrigger();

            var force = new Vector3(-Mathf.Cos(pow.angle), 0f, Mathf.Sin(pow.angle)) * pow.scale * skill_power_effect;
            GetComponent<Rigidbody>().AddForce(force);

            isClicked = false;
            interval = Time.time - interval;
            ReportManager.ReportTrialTime(interval);
        }
        else if (cur_state == state.running && isClicked)
        {
            // 화살표 UI를 없앤다
            Destroy(arrow);
            isClicked = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioSourceManager.instance.Play("bump", Bump);
        
        if (skill_script_power_up.triggered)
        {
            if (collision.gameObject.tag == "Stone")
            {
                Destroy(collision.gameObject);
            }
        }
    }


    public void Skill_power_up()
    {
        if (skill_script_power_up.triggered)
        {
            skill_power_effect = BOOST_SCALE;
        }
        else
        {
            skill_power_effect = 1f;
        }
    }

    public void Skill_score_double()
    {
        if (skill_script_score_double.triggered)
        {
            skill_score_effect = 2;
        }
        else
        {
            skill_power_effect = 1;
        }
    }


}
