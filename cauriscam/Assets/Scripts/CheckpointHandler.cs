using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointHandler : MonoBehaviour
{

    public List<Checkpoint> checkpoints;
    public TextMeshProUGUI checkpointText;

    public GameObject car;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //checkpoints.FindAll(checkpoint => checkpoint.isActive()).Count
    void Update()
    {
        var checkpointsActive = checkpoints.FindAll(checkpoint => checkpoint.isActive()).Count;
        var checkpointsTotal = checkpoints.Count;

        if (checkpointsActive == checkpointsTotal) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("DoneScene");
        }

        checkpointText.text = "Checkpoint: " + checkpointsActive + " / " + checkpointsTotal;
        if(Input.GetKeyDown(KeyCode.R)){
            SpawnToLastCheckpoint();
        }
    }

    void SpawnToLastCheckpoint()
    {
        // original spawn coordinates, remember to change!
        var spawnCoords = new Vector3(485.67f,1.65f,49.76f);
        var offset = new Vector3(0.0f, 10.0f, 0.0f); // otherwise clips through terrain
        var spawnRotation = new Quaternion(0.0f,-0.0933385864f,0.0f,0.995634437f);

        // loop through the coordinate list to find newest checkpoint
        for (int index = 0; index < checkpoints.Count; index++){
            if(checkpoints[index].isActive()){
                spawnCoords = checkpoints[index].transform.position + offset;
                spawnRotation = checkpoints[index].transform.rotation;
            }
        }

        // teleport car
        car.transform.position = spawnCoords;
        car.transform.rotation = spawnRotation;
        // code smell: checkpoint rotation is scuffed by 90 deg to left
        car.transform.rotation *= Quaternion.Euler(0, 270, 0);

    }
}
