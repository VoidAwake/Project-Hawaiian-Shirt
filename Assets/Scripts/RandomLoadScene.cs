using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomLoadScene : MonoBehaviour
{
    [SerializeField] int[] buildIndexes;

    void Start()
    {
        if (buildIndexes.Length > 0)
        {
            int random = Mathf.FloorToInt(Random.Range(0.001f, (float)buildIndexes.Length) - 0.001f);
            SceneManager.LoadScene(buildIndexes[random]);
        }
    }
}
