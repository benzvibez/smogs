using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;



public class AnimeController : MonoBehaviour
{
    public Animator animeController;
    public static AnimeController singleton;
    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        animeController = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void SwitchAnim(string anim, bool on)
    {
        if (anim == "idle-walk")
        {
            if (on)
            {
                animeController.SetBool("walking", true);
                animeController.SetBool("idle", false);
            }
            else
            {
                animeController.SetBool("walking", false);
                animeController.SetBool("idle", true);

            }
        }
    }
}
