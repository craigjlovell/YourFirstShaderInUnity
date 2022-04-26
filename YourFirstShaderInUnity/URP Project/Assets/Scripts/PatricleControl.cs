using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatricleControl : MonoBehaviour
{
    public ParticleSystem particle;

    public bool isParticle;
 
    public void Toggle()
    {
        if(isParticle == false)
        {
            particle.Play();
            isParticle = true;
        }
        else if(isParticle == true)
        {
            particle.Stop();
            isParticle = false;
        }
    }
}
