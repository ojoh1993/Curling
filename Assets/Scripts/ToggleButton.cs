using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleButton : MonoBehaviour {

    private Button b;
    public float alpha;
    public AudioSource SFX;
    public SkillContainer related;

    private bool clicked = false;

    private void Start()
    {
        b = GetComponent<Button>();
        b.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {/*
        EventSystem.current.SetSelectedGameObject(null);
        ColorBlock s = b.colors;
        var p = s.normalColor;
        s.normalColor = ToggledColor;
        ToggledColor = p;
        b.colors = s;*/
        if (related.skillUsed) return;

        buttonToggle();
        AudioSourceManager.instance.Play("charar",SFX);

    }

    void buttonToggle()
    {
        clicked = !clicked;
        Color c = GetComponent<Image>().color;
        float buf;
        buf = c.a;
        c.a = alpha;
        alpha = buf;
        GetComponent<Image>().color = c;
    }

    public void init()
    {
        if (clicked)
        {
            buttonToggle();
        }
    }
}
