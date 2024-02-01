using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private Difficulty sceneDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        sceneDifficulty = Difficulty.NoHelp;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDifficulty(Difficulty difficulty)
    {
        this.sceneDifficulty = difficulty;
    }

    public Difficulty GetDifficulty()
    {
        return this.sceneDifficulty;
    }
}
