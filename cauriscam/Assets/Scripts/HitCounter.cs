using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitCounter : MonoBehaviour
{

    private int hits;
    public TextMeshProUGUI hitText;

    // Start is called before the first frame update
    void Start()
    {
        hits = 0;
    }

    public void RegisterHit(){
        hits++;
    }

    // Update is called once per frame
    void Update()
    {
        hitText.text = hits + " accidents by far...";
    }
}
