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
	// Use this for initialization
	void Start ()
	{
	    delay = 0.5f;
	    trashList = new Dictionary<string, GameObject>();
	    trashList = trash.ToDictionary(x => x.name, x => x);

	}
	
	// Update is called once per frame
	void Update ()
    {
	    SpawnTrash();
	}

    public void SpawnTrash()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            delay = 0.5f;
            if (trashContainer.childCount < 10)
            {
                int trashID = Random.Range(0, trash.Count);
                int trashPointID = Random.Range(0, this.trashPoint.Count);
                GameObject go = Instantiate(trash[trashID], trashPoint[trashPointID].transform.position, Quaternion.identity) as GameObject;
                go.transform.SetParent(trashContainer);
                Vector3 pos = new Vector3(Random.Range(-10, 10), Random.Range(0, -5));
                TrashMove(go, pos);
            }
        }
    }

    public void TrashMove(GameObject trash, Vector3 pos)
    {
        MovingTrash movingTrash = trash.GetComponent<MovingTrash>();
        HOTween.To(trash.transform, 3f, new TweenParms()
            .Prop("position", pos)
            .OnComplete(movingTrash.Shake));
    }
}