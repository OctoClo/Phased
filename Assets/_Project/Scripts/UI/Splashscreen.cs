using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splashscreen : MonoBehaviour
{
    Animator animator;
    bool playedOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Play");
    }

    // Update is called once per frame
    void Update()
    {
        if (!playedOnce && this.animator.GetCurrentAnimatorStateInfo(0).IsName("01"))
        {
            playedOnce = true;
        }
        if(playedOnce && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("01")){
            SceneManager.LoadScene("MainScene");
        }
    }
}
