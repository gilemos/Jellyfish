using UnityEngine;
using Vuforia;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PROTECTED_MEMBER_VARIABLES

    public GameObject oxygenModel, oxygenExterior, hydrogenModel, hydrogenExterior, 
        waterModel, waterElectrosphere, cloud1, cloud2, cloud3;
    public bool isTracked = false;
    private static bool hasFoundWater = false;
    public GameObject awardCanvas;

    public static bool HasFoundWater
    {
        get
        {
            return hasFoundWater;
        }

        set
        {
            hasFoundWater = value;
        }
    }

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);

        waterModel.SetActive(false);
        waterElectrosphere.SetActive(false);
        oxygenModel.SetActive(false);
        oxygenExterior.SetActive(false);
        hydrogenModel.SetActive(false);
        hydrogenExterior.SetActive(false);
        awardCanvas.SetActive(false);
        cloud1.SetActive(false);
        cloud2.SetActive(false);
        cloud3.SetActive(false);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            isTracked = true;
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            isTracked = false;
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

        if(isBothTargetsTracked())
        {
            waterModel.SetActive(true);
            waterElectrosphere.SetActive(true);
            hydrogenModel.SetActive(false);
            hydrogenExterior.SetActive(false);
            oxygenModel.SetActive(false);
            oxygenExterior.SetActive(false);

            if(!HasFoundWater)
            {
                awardCanvas.SetActive(true);
                HasFoundWater = true;
            }
        }
        else
        {
            if(mTrackableBehaviour.TrackableName == "Oxygen")
            {
                oxygenModel.SetActive(true);
                oxygenExterior.SetActive(true);
            }
            if (mTrackableBehaviour.TrackableName == "Hydrogen")
            {
                hydrogenModel.SetActive(true);
                hydrogenExterior.SetActive(true);
            }
            if (mTrackableBehaviour.TrackableName == "Fissure")
            {
                Debug.Log("Got to fissure!! " + HasFoundWater);
                if (HasFoundWater)
                {
                    cloud1.SetActive(true);
                    cloud2.SetActive(true);
                    cloud3.SetActive(true);
                }
            }

        }
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;

        waterModel.SetActive(false);
        waterElectrosphere.SetActive(false);
        awardCanvas.SetActive(false);

        if (mTrackableBehaviour.TrackableName == "Hydrogen")
        {
            hydrogenModel.SetActive(false);
            hydrogenExterior.SetActive(false);

        }
        if (mTrackableBehaviour.TrackableName == "Oxygen")
        {
            oxygenModel.SetActive(false);
            oxygenExterior.SetActive(false);
        }
        if (mTrackableBehaviour.TrackableName == "Fissure")
        {
            cloud1.SetActive(false);
            cloud2.SetActive(false);
            cloud3.SetActive(false);
        }
    }

    private bool isBothTargetsTracked()
    {
        if (isTrackingMarker("ImageTargetOxygen") && isTrackingMarker("ImageTargetHydrogen")) {
            return true;
        }

        return false;
       // bool value = true;

       // foreach (DefaultTrackableEventHandler e in FindObjectsOfType<DefaultTrackableEventHandler>())
       // {
       //     if (!e.isTracked)
       //         value = false;
       // }
       // return value;
    }

    private bool isTrackingMarker(string imageTargetName)
    {
        print(imageTargetName);
        var imageTarget = GameObject.Find(imageTargetName);
        var trackable = imageTarget.GetComponent<TrackableBehaviour>();
        var status = trackable.CurrentStatus;
        return status == TrackableBehaviour.Status.TRACKED;
    }



    #endregion // PROTECTED_METHODS
}
