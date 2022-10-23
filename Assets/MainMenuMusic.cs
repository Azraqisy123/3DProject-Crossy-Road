using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
   public AudioSource source;
   public AudioClip clip;

   void Update()
   {
       if (Input.GetKeyDown(KeyCode.A))
       {
           source.PlayOneShot(clip);
       }
   }
}
