using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class PortalCamera : MonoBehaviour
{
    //Portal Stuff
    [SerializeField] private Portal[] portals = new Portal[2];
    [SerializeField] private Camera PortalCam;
    
    //iterations
    [SerializeField] private int iter = 9;
    
    //Render Stuff
    private RenderTexture tempTex0;
    private RenderTexture tempTex1;
    
    //Player Cam
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        tempTex0 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        tempTex1 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    }

    private void Start()
    {
        portals[0].GetComponent<Renderer>().material.mainTexture = tempTex0;
        portals[1].GetComponent<Renderer>().material.mainTexture = tempTex1;
    }

    private void OnEnable()
    {
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }

    private void UpdateCamera(ScriptableRenderContext SRC, Camera cam)
    {
        if (!portals[0].isPlaced || !portals[1].isPlaced)
        {
            return;
        }

        if (portals[0].GetComponent<Renderer>().isVisible)
        {
            PortalCam.targetTexture = tempTex0;
            for (int i = iter -1; i >= 0; --i)
            {
                RenderCamera(portals[0], portals[1], i, SRC);
            }
        }
        
        if (portals[1].GetComponent<Renderer>().isVisible)
        {
            PortalCam.targetTexture = tempTex1;
            for (int i = iter -1; i >= 0; --i)
            {
                RenderCamera(portals[1], portals[0], i, SRC);
            }
        }
    }

    private void RenderCamera(Portal portalIn, Portal portalOut, int iterationID, ScriptableRenderContext SRC)
    {
        Transform transformIn = portalIn.transform;
        Transform transformOut = portalOut.transform;

        Transform camTransform = PortalCam.transform;
        camTransform.position = transform.position;
        camTransform.rotation = transform.rotation;

        for (int i = 0; i <= iterationID; ++i)
        {
            Vector3 relativePos = transformIn.InverseTransformPoint(camTransform.position);
            relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
            camTransform.position = transformOut.TransformPoint(relativePos);

            Quaternion relativeRot = Quaternion.Inverse(transformIn.rotation) * camTransform.rotation;
            relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
            camTransform.rotation = transformOut.rotation * relativeRot;
        }

        Plane p = new Plane(-transformOut.forward, transformOut.position);
        Vector4 clipPtWS = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPtCS = Matrix4x4.Transpose(Matrix4x4.Inverse(PortalCam.worldToCameraMatrix)) * clipPtWS;

        var newM = mainCamera.CalculateObliqueMatrix(clipPtCS);
        PortalCam.projectionMatrix = newM;

        UniversalRenderPipeline.RenderSingleCamera(SRC, PortalCam);
    }
}
