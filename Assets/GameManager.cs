using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] Player player;
    [SerializeField] GameObject grass;

    [SerializeField] GameObject road;

    [SerializeField] int extent = 7;

    [SerializeField] int frontDistance = 10;

    [SerializeField] int BackDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;

    



    
    Dictionary<int,TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);
    TMP_Text gameOverText;

    private void Start()
    {
        //setup gameover panel
        gameOverPanel.SetActive(false);
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();



        //belakang
        for (int z = BackDistance; z <= 0; z++)
        {
            CreateTerrain(grass,z);
        }

        //depan
        for (int z = 1; z<= frontDistance; z++)
        {
           //block dengan probabilitas 50%
           var prefab = GetNextRandomTerrainPrefab(z);

           // instantiate block
           CreateTerrain(prefab, z);
        }

        player.SetUp (BackDistance, extent);
    }

    private int playerLastMaxTravel;


    private void Update()
    {
        // Cek player masih hidup/tidak
        if(player.IsDie && gameOverPanel.activeInHierarchy==false)
            StartCoroutine(ShowGameOverPanel());

        // Infinite Terrain System
        if(player.MaxTravel==playerLastMaxTravel)
            return;
        
        

        playerLastMaxTravel = player.MaxTravel;

        //bikin ke depan
        var randTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel + frontDistance);
        CreateTerrain(randTbPrefab,player.MaxTravel+frontDistance);

        //hapus yang dibelakang
        var lastTB = map[player.MaxTravel-1+BackDistance];

        //hapus dari daftar
        map.Remove(player.MaxTravel-1+BackDistance);

        //hilangkan dari scene
        Destroy(lastTB.gameObject);

        //setup player tidak bisa bergerak kebelakang
        player.SetUp(player.MaxTravel+BackDistance,extent);
            
    }

    IEnumerator ShowGameOverPanel()
    {   
        yield return new WaitForSeconds(3);

        Debug.Log("Game Over");
        gameOverText.text = "YOUR SCORE : " + player.MaxTravel;
        gameOverPanel.SetActive(true);
        Application.Quit();
    }

    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab,new Vector3(0,0,zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos, tb);
        Debug.Log(map[zPos]is Road);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos-1];
        for (int distance = 2; distance <= maxSameTerrainRepeat; distance++)
        {
            if(map[nextPos - distance].GetType()!=tbRef.GetType())
                {
                    isUniform = false;
                    break;
                }

        }

        if(isUniform)
        {
            if(tbRef is Grass)
                return road;
            else
                return grass;
        }
        //block dengan probabilitas 50%
        return Random.value > 0.5f ? road : grass;
    }



}
