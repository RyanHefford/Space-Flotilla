using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlordPointer : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;

    private Vector3 targetPosition;
    private GameObject overlord;
    private RectTransform pointerRectTransform;
    private OverlordManager overlordManager;

    private void Start()
    {
        Invoke("DelayedStart", 1f);
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerRectTransform.GetComponent<Image>().canvasRenderer.SetAlpha(0);
        overlordManager = GameObject.Find("Manager").GetComponent<OverlordManager>();
    }



    private void Update()
    {
        //check if overlord just spawned
        if (overlord == null && overlordManager.overlordExists)
        {
            //assign new overlord
            overlord = GameObject.FindGameObjectWithTag("Overlord");
        }

        if (overlord != null)
        {
            pointerRectTransform.GetComponent<Image>().canvasRenderer.SetAlpha(1);
            targetPosition = overlord.transform.position;

            Vector3 toPosition = targetPosition;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0f;
            toPosition.z = 0f;

            Vector3 dir = (toPosition - fromPosition).normalized;
            float angle = GetAngleFromVectorFloat(dir);


            pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);


            float borderSize = 40;
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
            bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize ||
                targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

            if (isOffScreen)
            {
                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
                if (cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
                if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
                if (cappedTargetScreenPosition.y >= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;

                Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                pointerRectTransform.position = pointerWorldPosition;
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
            }
            else
            {
                pointerRectTransform.GetComponent<Image>().canvasRenderer.SetAlpha(0);
            }
        }
        else
        {
            pointerRectTransform.GetComponent<Image>().canvasRenderer.SetAlpha(0);
        }

    }


    private void DelayedStart()
    {
        overlord = GameObject.FindGameObjectWithTag("Overlord");
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        return angle;
    }
}
