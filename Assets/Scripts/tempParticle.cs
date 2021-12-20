using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempParticle : MonoBehaviour
{
    private ParticleSystem ps;
    private void Awake()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ps.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
