using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectWithLineRenderers : MonoBehaviour
{
    public GameObject[] objectsToConnect = new GameObject[2];
    public bool connectLastToFirst = true;

    [Tooltip("Use of Alpha-Blend Particle Material recommendet.")]
    public Material lineMaterial;
    public float lineWith = 1;

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private List<Color32> objectColors = new List<Color32>();
    
    void Start()
    {
        foreach (var item in objectsToConnect)
        {
            lineRenderers.Add(item.AddComponent<LineRenderer>());
            objectColors.Add(item.GetComponent<Renderer>().material.color);
        }

        for (int i = 0; i < lineRenderers.Count; i++)
        {
            lineRenderers[i].startWidth = lineWith;
            lineRenderers[i].endWidth = lineWith;

            lineRenderers[i].material = lineMaterial;

            if (i < lineRenderers.Count - 1)
            {
                lineRenderers[i].startColor = objectColors[i];
                lineRenderers[i].endColor = objectColors[i + 1];
            }
            else if (connectLastToFirst)
            {
                lineRenderers[i].startColor = objectColors[i];
                lineRenderers[i].endColor = objectColors[0];
            }

            if (!connectLastToFirst)
                Destroy(lineRenderers[lineRenderers.Count - 1]);
        }
    }

    void Update()
    {
        UpdateLineRendererPositions();
    }


    void UpdateLineRendererPositions()
    {
        for (int i = 0; i < lineRenderers.Count; i++)
        {
            if (i < lineRenderers.Count - 1)
            {
                lineRenderers[i].SetPosition(0, objectsToConnect[i].transform.position);
                lineRenderers[i].SetPosition(1, objectsToConnect[i + 1].transform.position);
            }
            else if (connectLastToFirst)
            {
                lineRenderers[i].SetPosition(0, objectsToConnect[i].transform.position);
                lineRenderers[i].SetPosition(1, objectsToConnect[0].transform.position);
            }
        }
    }
}
