/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

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

    public GameObject oxygenModel, oxygenExterior, hydrogenModel, hydrogenExterior, waterModel, waterElectrosphere;
    public bool isTracked = false;
    private bool hasFoundWater = false;
    public GameObject awardCanvas;

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

            if(!hasFoundWater)
            {
                awardCanvas.SetActive(true);
                hasFoundWater = false;
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
    }

    private bool isBothTargetsTracked()
    {
        bool value = true;

        foreach (DefaultTrackableEventHandler e in FindObjectsOfType<DefaultTrackableEventHandler>())
        {
            if (!e.isTracked)
                value = false;
        }
        return value;
    }



    #endregion // PROTECTED_METHODS
}
