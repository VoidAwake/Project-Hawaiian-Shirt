using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderAnimator : MonoBehaviour
{
    // Some animation is done in BoulderTrap, but other animation is handled here
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem particlesEnter;
    [SerializeField] ParticleSystem particlesRolling;
    [SerializeField] ParticleSystem particlesExit;
    [SerializeField] SpriteRenderer spriteBoulder;
    [SerializeField] SpriteRenderer spriteShadow;

    // Start is called before the first frame update
    void OnEnable()
    {
        particlesRolling.Stop();
        particlesEnter.Stop();
        particlesExit.Stop();
        GetComponent<Collider2D>().enabled = false;
    }

    public void BeginRolling()
    {
        particlesRolling.Play();
    }

    public void PlayParticlesEnter()
    {
        particlesEnter.Play();
        GetComponent<Collider2D>().enabled = true;
    }

    public void PlayParticlesExit() // And destroy this boulder
    {
        spriteBoulder.enabled = false;
        spriteShadow.enabled = false;
        particlesRolling.Stop();
        particlesExit.Play();
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
