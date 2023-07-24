using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoneBeh : MonoBehaviour {
    [SerializeField]
    private int currentLevelId;

    private void OnTriggerEnter2D(Collider2D collision) {
        if ( collision.gameObject.tag == "Player" ) {
            SceneManager.LoadScene($"Level{currentLevelId + 1}Scene");
        }
    }
}
