using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;
    public static GameManager instance {
        get {
            return _instance;
        }
    }

    #endregion

    #region Fields

    public int worldSeed = 0;

    #endregion

    public void Start()
    {
        if( _instance == null ) {
            _instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
}
