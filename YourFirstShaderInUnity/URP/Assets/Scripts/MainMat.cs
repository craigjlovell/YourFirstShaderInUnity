using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMat : MonoBehaviour
{
    public Material[] mat;
    public GameObject player;
    public int x;
    SkinnedMeshRenderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        rend = player.transform.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>();
        rend.enabled = true;
        rend.material = mat[x];
    }

    // Update is called once per frame
    void Update()
    {
        rend.material = mat[x];
    }
    public void NextMat()
    {
        if(x < 2)
        {
            x++;
        }
        else
        {
            x = 0;
        }
    }

}
