using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;



public class AnimeController : MonoBehaviour
{
    public Animator animeController;
    // Start is called before the first frame update
    void Start()
    {
        animeController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var idle = animeController.GetBool("idle");
        var walking = animeController.GetBool("walking");
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (idle && !walking)
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
