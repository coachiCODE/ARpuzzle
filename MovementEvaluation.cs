using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class MovementEvaluation : MonoBehaviour
{
    public static MovementEvaluation Instance { set; get; }
    public GameObject ARcamera;
    public GameObject PanelCorrect;
    public GameObject PanelSequenceDone;
    public GameObject PanelRight;
    public GameObject PanelLeft;
    public GameObject PanelJump;

    public Vector3 pos;
    public Vector3 prevPos;
    private int evaluationRate;

    public int[] sequence;
    public int movementPositon; 

    private bool stepRight;
    private bool stepLeft;
    private bool stepForward;
    private bool stepBackward;
    private bool jump;
    private bool detected;

    private int detectedRate;

    // Start is called before the first frame update
    void Start()
    {
        hidePanel(PanelCorrect);
        hidePanel(PanelSequenceDone);
        hidePanel(PanelRight);
        hidePanel(PanelLeft);
        hidePanel(PanelJump);
        //hidePanel(PanelMovCorrect);


        Debug.Log(ARcamera.transform.position.ToString());
        pos = new Vector3(ARcamera.transform.position.x, ARcamera.transform.position.y, ARcamera.transform.position.z);
        Debug.Log("AR camera postition: " + pos);
        prevPos = pos;

        evaluationRate = 0;
        detectedRate = 0; 

        detected = false; 
        stepRight = false;
        stepLeft = false;
        stepForward = false;
        stepBackward = false;
        jump = false;

        //0: right, 1: left, 2: jump
        sequence = new int[] { 0, 1, 2, 1 };
        movementPositon = -1; 
        sequenceControl();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("AR camera postition: " + pos);
        pos.x = ARcamera.transform.position.x;
        pos.y = ARcamera.transform.position.y; 
        pos.z = ARcamera.transform.position.z;

 
        // Evaluate x position for step right and left
        if(pos.x - prevPos.x > 0.45)
        {
            if(stepRight == true){
                hidePanel(PanelRight);
                showPanel(PanelCorrect);
                stepRight = false;
                detected = true; 
            }       
        }
        if (pos.x - prevPos.x < -0.45)
        {
            if (stepLeft == true)
            {
                hidePanel(PanelLeft);
                showPanel(PanelCorrect);
                stepLeft = false;
                detected = true;
            }
        }

        // Evaluate y for jump
        if (pos.y - prevPos.y >= 0.3)
        {
            if (jump == true)
            {
                hidePanel(PanelJump);
                showPanel(PanelCorrect);
                jump = false;
                detected = true;
            }
        }

        // Evaluate z axis for step forward / backward
        if (pos.z - prevPos.x > 0.5)
        {
            stepForward = true;
        }
        if (pos.z - prevPos.x < -0.5)
        {
            stepBackward = true;
        }

        // Set the previous position every second (60 frames/s) and reset boolean
        if (evaluationRate == 60)
        {
            prevPos = pos;
            evaluationRate = 0;
        }

        // Show PanelCorrect for 2s before changing it to the next instruction
        if(detected == true)
        {
            detectedRate++; 

            if(detectedRate == 120)
            {
                hidePanel(PanelCorrect);
                sequenceControl();
                detectedRate = 0;
                detected = false; 
            }
        }
        
        evaluationRate++;
    }

    public void sequenceControl()
    {
        movementPositon++;

        if (movementPositon < sequence.Length)
        {
            if (sequence[movementPositon] == 0)
            {
                stepRight = true;
                showPanel(PanelRight);
            }
            if (sequence[movementPositon] == 1)
            {
                stepLeft = true;
                showPanel(PanelLeft);
            }
            if (sequence[movementPositon] == 2)
            {
                jump = true;
                showPanel(PanelJump);
            }
        }
        else
        {
            // Sequence finished
            showPanel(PanelSequenceDone);
        }
        
    }

    public void hidePanel(GameObject Panel)
    {
        Panel.gameObject.SetActive(false);
    }

    public void showPanel(GameObject Panel)
    {
        Panel.gameObject.SetActive(true);
    }

}
