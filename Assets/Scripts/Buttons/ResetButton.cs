using System;
using PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buttons
{
    public class ResetButton : MonoBehaviour
    {
        /*private void OnEnable()
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().onPlayerDead += () => gameObject.SetActive(true);
        }*/

        public void ResetScene()
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}