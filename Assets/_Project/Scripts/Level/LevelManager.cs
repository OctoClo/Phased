using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject Plane;
    public List<GameObject> LevelBricks;

    GameObject currentLevelBrickGO = null;
    LevelBrick currentLevelBrick;
    Vector3 brickPos;

    float offsetZFront;
    float offsetZBack;
    
    bool gameActive = false;

    private void Start()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.AddListener<GameEndEvent>(OnGameEndEvent);

        offsetZFront = Plane.transform.position.z + ((Plane.transform.localScale.z * 10) / 2);
        offsetZBack = Plane.transform.position.z - ((Plane.transform.localScale.z * 10) / 2);
    }

    private void Update()
    {
        if (gameActive)
        {
            if (LevelBricks.Count > 0)
            {
                if (!currentLevelBrickGO)
                {
                    CreateBrick();
                }
                else if (currentLevelBrick.HasSpawnedEverything)
                {
                    Destroy(currentLevelBrickGO);
                    LevelBricks.RemoveAt(0);

                    if (LevelBricks.Count > 0)
                    {
                        CreateBrick();
                    }
                }
            }
            else
            {
                EventManager.Instance.Raise(new GameEndEvent() { Victorious = true });
                gameActive = false;
            }
        }
    }

    void CreateBrick()
    {
        currentLevelBrickGO = Instantiate(LevelBricks[0]);
        currentLevelBrick = currentLevelBrickGO.GetComponent<LevelBrick>();
        if (currentLevelBrick.SpawnFromBack)
        {
            currentLevelBrick.ReverseSpawnables();
            brickPos = new Vector3(0, 0, offsetZBack - ((currentLevelBrick.Plane.transform.localScale.z * 10) / 2));
        }
        else
        {
            brickPos = new Vector3(0, 0, offsetZFront + ((currentLevelBrick.Plane.transform.localScale.z * 10) / 2));
        }
        
        currentLevelBrickGO.transform.position = brickPos;

        if (LevelBricks.Count == 1)
        {
            currentLevelBrick.WaitUntilBrickEnd();
        }
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        gameActive = true;
    }

    void OnGameEndEvent(GameEndEvent e)
    {
        gameActive = false;
    }
}
