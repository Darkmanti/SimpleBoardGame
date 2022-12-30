using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static WayPointData;
using UnityEngine.UIElements;

public class Route : MonoBehaviour
{
    [SerializeField] GameObject wayPointsParent;

    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject endPoint;

    WayPointData[] wayPoints;
    public List<GameObject> wayPointsSorted = new List<GameObject>();

    [Tooltip("Sort waypoints automatically")]
    [SerializeField] bool buildRouteAutomatically;

    private LineRenderer lineRenderer;


    void Start()
    {
        wayPoints = wayPointsParent.GetComponentsInChildren<WayPointData>();

        // for automatic building, if way points array doesn't sorted
        if(buildRouteAutomatically)
        {
            BuildRouteAuto();
        }
        else
        {
            BuildRouteAsItIs();
        }

        WayPointsSetInfo();

        // set debug lines
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    void Update()
    {
        DrawDebugLines();
    }


    void BuildRouteAuto()
    {
        Vector3 startPointPos = startPoint.transform.position;
        Vector3 endPointPos = endPoint.transform.position;

        float nearestStartPointPos = Mathf.Infinity;
        float nearestEndPointPos = Mathf.Infinity;

        int nearestStartPointId = 0;
        int nearestEndPointId = 0;
        float distance;

        int count = wayPoints.Count();

        // Find nearest point to start point
        // scip first object (this object)
        for (int i = 0; i < count; i++)
        {
            distance = Vector3.Distance(wayPoints[i].transform.position, startPointPos);
            if (nearestStartPointPos > distance)
            {
                nearestStartPointPos = distance;
                nearestStartPointId = i;
            }

            distance = Vector3.Distance(wayPoints[i].transform.position, endPointPos);
            if (nearestEndPointPos > distance)
            {
                nearestEndPointPos = distance;
                nearestEndPointId = i;
            }
        }

        wayPointsSorted.Add(startPoint);
        wayPointsSorted.Add(wayPoints[nearestStartPointId].gameObject);

        float nearestPos = Mathf.Infinity;
        int nearestId = 0;
        int currentId = nearestStartPointId;
        int prevId = 0;

        for(int i = 0; i < count - 1; i++)
        {
            for (int j = 0; j < count; j++)
            {
                if ((j == prevId) || (j == currentId))
                {
                    // Debug.Log("We catch this!");
                    // This is penultimate way point
                }
                else
                {
                    distance = Vector3.Distance(wayPoints[j].transform.position, wayPoints[currentId].transform.position);
                    if (nearestPos > distance)
                    {
                        nearestPos = distance;
                        nearestId = j;
                    }
                }
            }

            wayPointsSorted.Add(wayPoints[nearestId].gameObject);

            // clear variables
            nearestPos = Mathf.Infinity;
            prevId = currentId;
            currentId = nearestId;
            nearestId = 0;
        }

        wayPointsSorted.Add(endPoint);
    }

    void BuildRouteAsItIs()
    {
        wayPointsSorted.Add(startPoint);

        foreach(WayPointData item in wayPoints)
        {
            wayPointsSorted.Add(item.transform.gameObject);
        }

        wayPointsSorted.Add(endPoint);
    }

    void DrawDebugLines()
    {
        int count = wayPointsSorted.Count;

        lineRenderer.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            lineRenderer.SetPosition(i, wayPointsSorted[i].transform.position);
        }
    }

    void WayPointsSetInfo()
    {
        int count = wayPointsSorted.Count;

        TextMeshPro TMPComponent;
        TMPComponent = wayPointsSorted[0].GetComponentInChildren<TextMeshPro>(true);
        TMPComponent.GameObject().SetActive(true);
        TMPComponent.SetText("Start");

        wayPointsSorted[0].GetComponentInChildren<TextMeshPro>(true).text = "Start";

        for (int i = 1; i < count - 1; i++)
        {
            TMPComponent = wayPointsSorted[i].GetComponentInChildren<TextMeshPro>(true);
            TMPComponent.GameObject().SetActive(true);

            WayPointData pointData = wayPointsSorted[i].GetComponent<WayPointData>();

            if (pointData.pointType == WayPointEnumerator.Point)
            {
                switch(pointData.bonusType)
                {
                    case WayPointBonus.Default:
                        {
                            pointData.pointInfo = i.ToString();
                            break;
                        }

                    case WayPointBonus.Fail:
                        {
                            pointData.pointInfo = "-" + pointData.bonusValue.ToString();
                            break;
                        }

                    case WayPointBonus.Buff:
                        {
                            pointData.pointInfo = "+" + pointData.bonusValue.ToString();
                            break;
                        }

                    case WayPointBonus.OneTurn:
                        {
                            pointData.pointInfo = "+TURN";
                            break;
                        }
                }
            }

            TMPComponent.SetText(pointData.pointInfo);

        }

        TMPComponent = wayPointsSorted[count - 1].GetComponentInChildren<TextMeshPro>(true);
        TMPComponent.GameObject().SetActive(true);
        TMPComponent.SetText("End");
    }

}
