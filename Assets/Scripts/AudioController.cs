using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    public AudioSource lockOn;
    public AudioSource thrust;
    public AudioSource shieldHit;
    public AudioSource shootProjectile;
    public AudioSource shootLaser;
    public AudioSource death;
    public AudioSource drag;
    public AudioSource hullHit;
    public AudioSource laserPulse;
    public AudioSource megaLaser;


    public IEnumerator VolumeFade(AudioSource _AudioSource, float _EndVolume, float _FadeLength)
    {

        float _StartVolume = _AudioSource.volume;

        float _StartTime = Time.time;

        while (Time.time < _StartTime + _FadeLength)
        {

            _AudioSource.volume = _StartVolume + ((_EndVolume - _StartVolume) * ((Time.time - _StartTime) / _FadeLength));

            yield return null;

        }

        if (_EndVolume == 0)
        {
            _AudioSource.Stop();
            _AudioSource.volume = 100;
        }

    }

    public void playLockOn()
    {
        if (!lockOn.isPlaying)
            lockOn.Play();
    }

    public void fadeThrust()
    {
        StartCoroutine(VolumeFade(thrust, 0f, (float)0.2));
    }

    public void playThrust()
    {
        if (!thrust.isPlaying)
            thrust.Play();
    }

    public void playShieldHit()
    {
        if (!shieldHit.isPlaying)
            shieldHit.Play();
    }

    public void playShootProjectile()
    {
        shootProjectile.Play();
    }

    public void playShootLaser()
    {
        if (!shootLaser.isPlaying)
            shootLaser.Play();
    }

    public void fadeShootLaser()
    {
        StartCoroutine(VolumeFade(shootLaser, 0f, (float)0.3));
    }

    public void playDeath()
    {
        death.Play();
    }

    public void playInertialDampener()
    {
        if (!drag.isPlaying)
            drag.Play();
    }

    public void playHullHit()
    {
        hullHit.Play();
    }

    public void playShootLaserPulse()
    {
        laserPulse.Play();
    }

    public void playMegaLaser()
    {
        megaLaser.Play();
    }
}
