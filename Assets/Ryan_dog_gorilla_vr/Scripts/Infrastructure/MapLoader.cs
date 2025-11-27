using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    // TODO this should be in some kind of build sript to ensure these scense are included in the build.
    private static readonly List<String> MAPS = new List<String>
    {
        "map_spooky",
        "map_spooky_climb",
    };

    void Awake()
    {
        foreach (String map in MAPS)
        {
            Debug.Log($"Loading map {map}");
            SceneManager.LoadScene(map, LoadSceneMode.Additive);
        }

    }

}
