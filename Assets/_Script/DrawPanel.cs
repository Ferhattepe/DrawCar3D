﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using PathCreation;
using ProBuilder2.Common;
using ProBuilder2.MeshOperations;
public class DrawPanel : MonoBehaviour
{
    #region //----> Variable
    public RaksTouch touch;
    public UILineTextureRenderer lineRenderer;
    [HideInInspector]
    public List<Vector2> fingerPositions = new List<Vector2>();
    RaksTimer timer = new RaksTimer();
    public Transform startPos;
    public Material drawCarMaterial;
    #endregion
    #region //----> Unity Method
    // Start is called before the first frame update
    void Start()
    {
        //pathCreator.pat
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (touch.isEvent)
            {
                CreateLine();
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (touch.isEvent)
            {
                UpdateLine();
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            CreateDrawCar(fingerPositions);
        }
    }
    #endregion
    #region //----> My Method
    /// <summary>
    /// İlk tıklamada Oluşturulacak olan işlemleri burada belirtiyoruz .
    /// </summary>
    /// 
    public void CreateLine()
    {
        fingerPositions.Clear();
        Vector2 firstPos = lineRenderer.transform.InverseTransformPoint(touch.startPosition);
        fingerPositions.Add(firstPos);
        //lineRenderer.transform.position = touch.startPosition;
        lineRenderer.Points = fingerPositions.ToArray();
    }
    public void UpdateLine()
    {
        float dis =  touch.deltaPosition.magnitude;
        Debug.Log("Deneme : "+ dis);
        if (dis > Screen.width*.01f)
        {
            Vector2 fingerPos = lineRenderer.transform.InverseTransformPoint(touch.currentPosition);
            fingerPositions.Add(fingerPos);
            lineRenderer.Points = fingerPositions.ToArray();
        }
    }
    public void CreateDrawCar(List<Vector2> points)
    {
        GameObject pathObjectContainer = new GameObject();
        var pathO = pathObjectContainer.AddComponent<pb_Object>();
        var pbEntity = pathObjectContainer.GetComponent<pb_Entity>();
        //pathObjectContainer.AddComponent<pb_Entity>();
        var pathObject = pathObjectContainer.AddComponent<pb_BezierShape>();
        List<pb_BezierPoint> pathPoints = new List<pb_BezierPoint>();
        for (int i = 0; i < points.Count; i++)
        {
            pathPoints.Add(new pb_BezierPoint(points[i], points[i], points[i], Quaternion.identity));
            var sC = pathObjectContainer.AddComponent<SphereCollider>();
            //sC.radius = 10;
            //sC.center = points[i];
        }
        pathObject.m_Points = pathPoints;
        pathObject.m_Radius = 10;
        pathObject.Refresh();
        //pathObjectContainer.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        pathObjectContainer.transform.position = startPos.position;
        pathO.ToMesh();
        //pathO.
        Mesh m = pathObjectContainer.GetComponent<MeshFilter>().sharedMesh;
        MeshRenderer mR = pathObjectContainer.GetComponent<MeshRenderer>();
        mR.sharedMaterial = drawCarMaterial;
        //pathObjectContainer.AddComponent<MeshCollider>().sharedMesh = m;
        pathObjectContainer.AddComponent<Rigidbody>();
    }

    #endregion
}
