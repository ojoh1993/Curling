using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillContainer : MonoBehaviour
{
    public string skillName;
    public bool triggered;
    public bool skillUsed;
    public Button related;
    UnityAction Act;

    public void SkillRegister(UnityAction act) { 
        EventManager.StartListening(skillName, act);
        Act = act;
    }

    public void SkillTrigger()
    {
        if (!skillUsed && triggered)
        {
            EventManager.TriggerEvent(skillName);
            skillUsed = true;
            if (related != null)
                related.gameObject.SetActive(false);
        }
    }

    public void OnDestroy()
    {
        EventManager.StopListening(skillName,Act);
    }

    public void skillInit()
    {
        related.gameObject.SetActive(true);
        triggered = false;
        skillUsed = false;
    }

    
}


