using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointHandler : MonoBehaviour
{

    public List<Checkpoint> checkpoints;
    public TextMeshProUGUI checkpointText;


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

        //if (checkpointsActive == checkpointsTotal) {
        //    // JOTAIN HAUSKAA
        //}

        checkpointText.text = "Checkpoint: " + checkpointsActive + " / " + checkpointsTotal;
    }
}
