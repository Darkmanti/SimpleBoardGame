using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WayPointData : MonoBehaviour
{
    [SerializeField] public TextMeshPro TMPComponent;

    public enum WayPointEnumerator
    {
        Start,
        End,
        Point
    }

    public enum WayPointBonus
    {
        Default,
        Fail,
        Buff,
        OneTurn
    }

    public WayPointEnumerator pointType;

    [SerializeField] public WayPointBonus bonusType = WayPointBonus.Default;
    [SerializeField] public int bonusValue = 0;
    public string pointInfo;

    private void OnValidate()
    {
        if (pointType == WayPointEnumerator.Point)
        {
            if (bonusType == WayPointBonus.Fail)
            {
                TMPComponent.color = new Vector4(0.5f, 0.0f, 0.0f, 1.0f);
                TMPComponent.SetText("-" + bonusValue.ToString());
            }
            else if (bonusType == WayPointBonus.Buff)
            {
                TMPComponent.color = new Vector4(0.0f, 0.0f, 0.5f, 1.0f);
                TMPComponent.SetText("+" + bonusValue.ToString());
            }
            else if (bonusType == WayPointBonus.Default)
            {
                TMPComponent.color = new Vector4(0.0f, 0.5f, 0.0f, 1.0f);
                TMPComponent.SetText("10");
            }
            else if (bonusType == WayPointBonus.OneTurn)
            {
                TMPComponent.color = new Vector4(0.0f, 0.5f, 0.0f, 1.0f);
                TMPComponent.SetText("+");
            }
        }
        else if (pointType == WayPointEnumerator.Start)
        {
            TMPComponent.SetText("Start");
        }
        else if (pointType == WayPointEnumerator.End)
        {
            TMPComponent.SetText("End");
        }


    }
}
