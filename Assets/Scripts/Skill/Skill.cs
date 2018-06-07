using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Skill : MonoBehaviour
{
    public SkillContainer SKILL_POWER_UP;
    public SkillContainer SKILL_SCORE_DOUBLE;


    private static Skill skill;

    public static Skill instance
    {
        get
        {
            if (!skill)
            {
                skill = FindObjectOfType(typeof(Skill)) as Skill;

                if (!skill)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    skill.Init();
                }
            }

            return skill;
        }
    }

    public void Init()
    {

    }
}

