﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holoville.HOTween;

public class TrashSpawn : MonoBehaviour
{
    public List<GameObject> trashPoint;
    public Transform trashContainer; 

    public List<GameObject> trash;
    public Dictionary<string, GameObject> trashList;
    public float delay;
    public int startTrash;

    public bool isSpawn;
	// Use this for initialization
	void Start ()
	{
	    if (GameManager.Instance.gameModeConfig == GameModeConfig.GAME_OFFLINE)
	    {
	        isSpawn = true;
	    }
	    else
	    {
	        isSpawn = false;
	    }
	    delay = 0.3f;
	    trashList = new Dictionary<string, GameObject>();
	    trashList = trash.ToDictionary(x => x.name, x => x);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (isSpawn)
	    {
            if (startTrash > 0)
            {
                InstantTrash();
                startTrash--;
            }
            else
            {
                if (startTrash == 0)
                {
                    startTrash--;
                    delay = 3;
                }
            }
            SpawnTrash();
        }
	}

    public void SpawnTrash()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            delay = 5f;
            if (trashContainer.childCount < 20)
            {
                InstantTrash();
                //kill fish4
                
            }
        }
    }

    public void InstantTrash()
    {
        int trashID = Random.Range(0, trash.Count);
        int trashPointID = Random.Range(0, this.trashPoint.Count);
        GameObject go;

        Vector3 pos = new Vector3(Random.Range(-8f, 8f), Random.Range(0f, -5f));
        if (GameManager.Instance.gameModeConfig == GameModeConfig.GAME_OFFLINE)
        { 
            go = Instantiate(trash[trashID], trashPoint[trashPointID].transform.position, Quaternion.identity) as GameObject;
            go.transform.SetParent(trashContainer);
            
        }
        else
        {
            go = PhotonNetwork.Instantiate(trash[trashID].name, trashPoint[trashPointID].transform.position, Quaternion.identity, 0) as GameObject;

            //photonView.RPC("OnMove", PhotonTargets.AllBufferedViaServer, new object[] { go, pos });
        }

        TrashMove(go, pos);
       

        //Kill fish
        GameManager.Instance.gameMode.AddTrash(go);
    }

    void OnMove(GameObject go, Vector3 pos)
    { 
        
    }



    public void TrashMove(GameObject trash, Vector3 pos)
    {
        MovingTrash movingTrash = trash.GetComponent<MovingTrash>();
        if (GameManager.Instance.gameModeConfig == GameModeConfig.GAME_OFFLINE)
        {
            movingTrash.OnMove(pos);
        }else{
            movingTrash.MoveTrash(pos);
        }   

        
           // /*.OnComplete(movingTrash.Shake)*/);
    }
}
