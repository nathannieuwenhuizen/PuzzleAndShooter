using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : PoolObject
{
    private ParticleSystem ps;
    private AudioSource audios;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        audios = GetComponent<AudioSource>();
    }

    public override void OnObjectReuse()
    {
        ps.Play();
        audios.Play();
    }
}
