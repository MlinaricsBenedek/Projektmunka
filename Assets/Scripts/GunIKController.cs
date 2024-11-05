using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunIKController : MonoBehaviour
{

    [SerializeField] Animator gunAnim;
    [SerializeField] GameObject Head;
    [SerializeField] GameObject LeftArm;
    [SerializeField] GameObject RightArm;
    [SerializeField] Transform targetPosition;
    public bool isIKActive = true;
    private void OnAnimatorIK(int layerIndex)
    {
        if (gunAnim)
        {// Set the look target position, if one has been assigned
            if (targetPosition != null)
            {
                gunAnim.SetLookAtWeight(1);
                gunAnim.SetLookAtPosition(targetPosition.position);
            }

            // Set the right hand target position and rotation, if one has been assigned
          
                gunAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, RightArm.transform.position.y);
                gunAnim.SetIKRotationWeight(AvatarIKGoal.RightHand, RightArm.transform.rotation.y);
            
                gunAnim.SetIKPosition(AvatarIKGoal.RightHand, targetPosition.transform.position);
                gunAnim.SetIKRotation(AvatarIKGoal.RightHand, targetPosition.transform.rotation);

            gunAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, LeftArm.transform.position.y);
            gunAnim.SetIKRotationWeight(AvatarIKGoal.LeftHand, LeftArm.transform.rotation.x);
            //gunAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, LeftArm.transform.position.z);
            gunAnim.SetIKRotationWeight(AvatarIKGoal.LeftHand, LeftArm.transform.rotation.y);
            //gunAnim.SetIKRotationWeight(AvatarIKGoal.LeftHand, LeftArm.transform.rotation.z);
            // Bal kéz rotációs súly beállítása

            // Bal kéz pozícióját és rotációját az illeszkedéshez beállítod
            gunAnim.SetIKPosition(AvatarIKGoal.LeftHand, targetPosition.transform.position);
            gunAnim.SetIKRotation(AvatarIKGoal.LeftHand, targetPosition.transform.rotation);
        }

        //if the IK is not active, set the position and rotation of the hand and head back to the original position
        else
        {
            gunAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            gunAnim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            gunAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            gunAnim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            gunAnim.SetLookAtWeight(0);

        }
    }


}

