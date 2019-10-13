﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Called in a scene by itself immediately after the game is launched;
// for setting up data needed before the main menu
public class GameLaunchManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GroundMaterialLibrary.LoadLibrary();
		SceneManager.LoadScene((int)UnityScenes.Menu);
    }

}