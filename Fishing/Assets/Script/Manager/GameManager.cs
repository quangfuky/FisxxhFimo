﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public GameModeConfig gameModeConfig;

    //[HideInInspector]
    public GameMode gameMode;

    const float TIME_UPDATE = 0.1f;
    float timeGame;

    public Transform[] Spots;

    // [HideInInspector]
    public int[] listSpots;

    public GameObject prefabBoat;
    public GameObject prefabBoatMulti;

    public List<GameObject> fishPrefabs;

    public Text txtTimer;

    public int fish;
    public int trash;

    public int fishCountCollect;
    public int trashCountCollect;

    //Trash Spawm
    public TrashSpawn trashSpawn;

    //UI MUlti
    //public GameObject bntStartGame;
    //public GameObject txtWaiting;

    public bool isUpdateData = false;
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        isUpdateData = false;
        trashSpawn.isSpawn = false;

        fishCountCollect = 0;
        trashCountCollect = 0;
        
        StartGame();
    }

    public void SetUICount()
    {
        GameObject.Find("FishScore").GetComponent<Text>().text = fishCountCollect.ToString();
        GameObject.Find("TrashScore").GetComponent<Text>().text = trashCountCollect.ToString();
    }

    void StartGame()
    {
        //int currentLV = PlayerPrefs.GetInt("PlayLevel", 0);
        listSpots = new int[Spots.Length];
        switch (gameModeConfig)
        {
            case GameModeConfig.GAME_OFFLINE:
                gameMode = new GameOffline();
                break;
            case GameModeConfig.GAME_MULTI_VS:
                //currentLV = Random.Range(0, 40);
                gameMode = new GameMulti();

                //if (PhotonNetwork.isMasterClient)
                //{
                //    bntStartGame.SetActive(true);
                //    txtWaiting.SetActive(false);
                //}
                //else
                //{
                //    bntStartGame.SetActive(false);
                //    txtWaiting.SetActive(true);
                //}
                break;
        }
        //Debug.Log("Load level play here");
        //timeGame = 60;

        gameMode.StartGame();
        StartCoroutine(StartGameByFrame());
        SetUICount();
    }

    public void SetGameState(GameState gameState)
    {
        gameMode.gameState = gameState;
    }

    public void DisableUIMulti()
    {
        //bntStartGame.SetActive(false);
        //txtWaiting.SetActive(false);
    }

    public void SetTxtTimer(float time)
    {
        //txtTimer.text = "0:" + ((int)time).ToString();
    }

    IEnumerator StartGameByFrame()
    {
        yield return new WaitForSeconds(TIME_UPDATE);

        gameMode.UpdateGame(TIME_UPDATE);
        SetTxtTimer(gameMode.timeGame);

        StartCoroutine(StartGameByFrame());
    }

    #region SPOTS
    public int GetAvailableSpot()
    {
        for (int i = 0; i < listSpots.Length; i++)
        {
            if (listSpots[i] == 0)
            {
                return i;
            }
        }
        return 0;
    }

    public bool CheckSpotAvailable(int index)
    {
        if (listSpots[index] == 0)
        {
            return true;
        }
        return false;
    }


    //When other player out game, reset his index
    public void ResetSpot(int index)
    {
        listSpots[index] = 0;
    }

    public void SetSpotByIndex(int index, int old_index, Transform child, bool hasChanged)
    {
        if (old_index >= 0 && old_index < listSpots.Length)
        {
            listSpots[old_index] = 0;
        }
        if (index >= 0 && index < listSpots.Length)
        {
            listSpots[index] = 1;
        }
        //Syn to server

        if (hasChanged)
        {
            //ExitGames.Client.Photon.Hashtable roomProp = new ExitGames.Client.Photon.Hashtable() { { "spots", listSpots } };
            //PhotonNetwork.room.SetCustomProperties(roomProp);
            //PhotonNetwork.room.c
        }
        //Debug.Log("GG " + index + " ---- " + old_index);
        if (old_index != index)
        {
            Debug.Log("Old index = " + old_index + " - index = " + index);
            if (index >= 0 && index < listSpots.Length)
            {
                if (Spots[index].childCount == 0)
                {
                    //Set parent
                    child.parent = Spots[index];
                }
                else
                {
                    Debug.Log("Can't add child with not availible spot");
                }
            }
            else
            {
                Debug.Log("List spot not availible");
            }

        }
    }
    #endregion

    public GameObject CreateFishByNameOffline(int iDFish)
    {
        //Debug.Log("Create Fish name = " + fishName);
        if (this.fishPrefabs == null)
        {
            Debug.Log("Fuck");
            return null;
        }
        if (iDFish < fishPrefabs.Count)
        {
            //Debug.Log("Contained key");
            return Instantiate(this.fishPrefabs[iDFish], Vector3.zero, Quaternion.identity) as GameObject;
        }
        else
        {
            Debug.Log("FishObj not contain fish = " + iDFish);
            return null;
        }

    }

    public void RemoveFishFromList(FishInfo fishInfo, GameObject fish)
    {
        gameMode.RemoveFish(fishInfo, fish);
    }

}
