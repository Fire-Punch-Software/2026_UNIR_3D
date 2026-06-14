using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneController : MonoBehaviour
{
    [Header("Camera References")]
    public GameObject camera1;
    public GameObject camera2;

    public GameObject playerCamera;

    [Header("Animation References")]
    public Animator animCamera1;
    public Animator animCamera2;

    private bool camera1Finished = false;
    private bool camera2Finished = false;

    void Start()
    {
        // Start the first camera animation
        animCamera1 = camera1.GetComponent<Animator>();
        animCamera2 = camera2.GetComponent<Animator>();

        ActiveCamera(camera1);
        playerCamera.SetActive(false);
    }

    void Update()
    {
        // Check if camera1 animation has finished
        if (camera1.activeSelf && !camera1Finished && animCamera1.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !animCamera1.IsInTransition(0))
        {
            camera1Finished = true;
            ActiveCamera(camera2);
        }
        // Check if camera2 animation has finished
        if (camera2.activeSelf && !camera2Finished && animCamera2.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !animCamera2.IsInTransition(0))
        {
            camera2Finished = true;
            camera2.SetActive(false);

            playerCamera.SetActive(true);
        }
    }

    void ActiveCamera(GameObject cameraToActivate)
    {
        camera1.SetActive(cameraToActivate == camera1);
        camera2.SetActive(cameraToActivate == camera2);
        playerCamera.SetActive(false);

        if(cameraToActivate == camera1) camera1Finished = false;
        if(cameraToActivate == camera2) camera2Finished = false;
    }
}
