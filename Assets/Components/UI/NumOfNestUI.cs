using UnityEngine;
using UnityEngine.UI;

public class NumOfNestUI : MonoBehaviour
{

    public Text output;
    private int nestBlockNum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nestBlockNum = 0;
        output.text = "Nest blocks: " + nestBlockNum;
    }

    // Update is called once per frame
    void Update()
    {
        //nestBlockNum++;
        output.text = "Nest blocks: " + nestBlockNum;
    }
}
