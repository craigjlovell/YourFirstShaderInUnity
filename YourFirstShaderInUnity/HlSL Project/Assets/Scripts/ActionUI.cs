using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionUI : MonoBehaviour
{
    public Action action;
    public Player player;

    [Header("Child Components")]
    public Image icon;
    public TextMeshProUGUI nameTag;
    public TextMeshProUGUI desccriptionTag;

    public void SetAction(Action a)
    {
        if (a != null)
        {
            action = a;
            if (nameTag)
                nameTag.text = a.actionName;
            if (desccriptionTag)
                desccriptionTag.text = action.description;
            if (icon)
            {
                icon.sprite = action.icon;
                icon.color = action.color;
            }
        }
    }

    public void Init(Player p)
    {
        player = p;
        Button button = GetComponentInChildren<Button>();
        if (button)
            button.onClick.AddListener(() => { player.DoAction(action); });
    }

    private void Start()
    {
        SetAction(action);
    }
}
