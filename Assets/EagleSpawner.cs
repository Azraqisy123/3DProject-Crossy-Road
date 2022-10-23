using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSpawner : MonoBehaviour
{
    [SerializeField] GameObject eaglePrefab;
    [SerializeField] int spawnZPos = 7;
    [SerializeField] Player player;
    [SerializeField] float timeout = 5;

    [SerializeField] float timer = 0;
    [SerializeField] private AudioSource EagleSoundEffect;

    int playerLastMaxTravel=0;
    private void Start()
    {
        
    }

    private void SpawnEagle()
    {   
        player.enabled = false;
        var position = new Vector3(player.transform.position.x, 1, player.CurrentTravel + spawnZPos);
        var rotation = Quaternion.Euler(0,180,0);
        var eagleObject = Instantiate(eaglePrefab, position, rotation);
        var eagle = eagleObject.GetComponent<Eagle>();
        eagle.SetUpTarget(player);
        EagleSoundEffect.Play();
    }

    private void Update()
    {
        if(player.MaxTravel != playerLastMaxTravel)
        {
            timer = 0;
            playerLastMaxTravel = player.MaxTravel;
            return;
        }

        if(timer < timeout)
        {
            timer += Time.deltaTime;
            return;

        }

        if(player.IsJumping()==false && player.IsDie == false)
            SpawnEagle();
            
    }

}
