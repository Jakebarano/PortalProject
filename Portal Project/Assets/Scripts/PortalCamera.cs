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
    [SerializeField] private int iterations = 7;
    
    //Render Stuff
    private RenderTexture tempTex0;
    private RenderTexture tempTex1;
    
    //Cams
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        tempTex0 = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        tempTex1 = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
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
        if (!portals[0].IsPlaced || !portals[1].IsPlaced)
        {
            return;
        }

        if (portals[0].GetComponent<Renderer>().isVisible)
        {
            PortalCam.targetTexture = tempTex0;
            for (int i = iterations -1; i >= 0; --i)
            {
                RenderCamera(portals[0], portals[1], i, SRC);
            }
        }
        
        if (portals[1].GetComponent<Renderer>().isVisible)
        {
            PortalCam.targetTexture = tempTex0;
            for (int i = iterations -1; i >= 0; --i)
            {
                RenderCamera(portals[1], portals[0], i, SRC);
            }
        }
    }

    private void RenderCamera(Portal inPortal, Portal outPortal, int iterationID, ScriptableRenderContext SRC)
    {
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        Transform camTransform = PortalCam.transform;
        camTransform.position = transform.position;
        camTransform.rotation = transform.rotation;

        for (int i = 0; i <= iterationID; ++i)
        {
            Vector3 relativePos = inTransform.InverseTransformPoint(camTransform.position);
            relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
            camTransform.position = outTransform.TransformPoint(relativePos);

            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * camTransform.rotation;
            relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
            camTransform.rotation = outTransform.rotation * relativeRot;
        }

        Plane p = new Plane(-outTransform.forward, outTransform.position);
        Vector4 clipPtWS = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPtCS = Matrix4x4.Transpose(Matrix4x4.Inverse(PortalCam.worldToCameraMatrix)) * clipPtWS;

        var newMatrix = mainCamera.CalculateObliqueMatrix(clipPtCS);
        PortalCam.projectionMatrix = newMatrix;

        UniversalRenderPipeline.RenderSingleCamera(SRC, PortalCam);
    }
}
