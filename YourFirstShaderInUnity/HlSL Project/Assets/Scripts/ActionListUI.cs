using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionListUI : MonoBehaviour
{
    LayoutGroup layoutGroup;
    ContentSizeFitter contentSizeFitter;

    public ActionList actionList;
    public ActionUI prefab;
    //Player player;

    List<ActionUI> uis = new List<ActionUI>();
    // Start is called before the first frame update
    //void Start()
    //{
    //
    //    contentSizeFitter = GetComponent<ContentSizeFitter>();
    //    layoutGroup = GetComponent<LayoutGroup>();
    //    StartCoroutine(UpdateUI());
    //
    //    actionList.onChanged.AddListener(() => { StartCoroutine(UpdateUI()); });
    //}
    //
    //IEnumerator UpdateUI()
    //{
    //    contentSizeFitter.enabled = true;
    //    layoutGroup.enabled = true;
    //    yield return new WaitForEndOfFrame();
    //
    //    player = actionList.GetComponent<Player>();
    //
    //    for (int i = 0; i < actionList.actions.Length; i++)
    //    {
    //        if (i > uis.Count)
    //        {
    //            uis.Add(Instantiate(prefab, transform));
    //            uis[i].Init(player);
    //        }
    //        uis[i].gameObject.SetActive(true);
    //        uis[i].SetAction(actionList.actions[i]);
    //
    //        uis[i].transform.SetAsLastSibling();
    //    }
    //
    //    for (int i = actionList.actions.Length; i < uis.Count; i++)
    //        uis[i].gameObject.SetActive(false);
    //    yield return new WaitForEndOfFrame();
    //
    //    contentSizeFitter.enabled = false;
    //    layoutGroup.enabled = false;
    //}

    IEnumerator Start()
    {
        Player player = actionList.GetComponent<Player>();
    
        foreach (Action a in actionList.actions)
        {
            // Make this a child of ours on creation. Don't worry about specifying a position as the LayotGroup handles that
            ActionUI ui = Instantiate(prefab, transform);
            ui.SetAction(a);
            ui.Init(player);
        }
        yield return new WaitForEndOfFrame();
    
        GetComponent<ContentSizeFitter>().enabled = false;
        GetComponent<LayoutGroup>().enabled = false;
    
    
    }
}
