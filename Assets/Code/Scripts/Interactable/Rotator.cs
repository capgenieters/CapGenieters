using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

namespace LevelUP.Dial
{
    public class Rotator : MonoBehaviour
    {
        public Transform linkedDial;
        
        [SerializeField] private int snapRotationAmout = 25;
        [SerializeField] private float angleTolerance;
        [SerializeField] private GameObject RighthandModel;
        [SerializeField] private GameObject LefthandModel;

        private XRBaseInteractor interactor;
        private float startAngle;
        private bool requiresStartAngle = true;
        private bool shouldGetHandRotation = false;

        public void GrabbedBy()
        {
            interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
            interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;

            shouldGetHandRotation = true;
            startAngle = 0f;

            HandModelVisibility(true);
        }

        private void HandModelVisibility(bool visibilityState)
        {
            if(interactor.gameObject.GetComponent<XRController>().controllerNode == XRNode.RightHand)
            {
                RighthandModel.SetActive(visibilityState);
            }
            else
            {
                LefthandModel.SetActive(visibilityState);
            }
        }

        public void GrabEnd()
        {
            shouldGetHandRotation = false;
            requiresStartAngle = true;
            HandModelVisibility(false);
        }

        void Update()
        {
            if (shouldGetHandRotation)
            {
                var rotationAngle = GetInteractorRotation(); //gets the current controller angle
                GetRotationDistance(rotationAngle);
            }
        }

        public float GetInteractorRotation()
        {
            var handRotation = interactor.GetComponent<Transform>().eulerAngles;
            return handRotation.z;
        }

        private void GetRotationDistance(float currentAngle)
        {
            if (!requiresStartAngle)
            {
                var angleDifference = Mathf.Abs(startAngle - currentAngle);

                if (angleDifference > angleTolerance)
                {
                    if (angleDifference > 270f) //checking to see if the user has gone from 0-360 - a very tiny movement but will trigger the angletolerance
                    {
                        float angleCheck;

                        if (startAngle < currentAngle) //going anticlockwise
                        {
                            angleCheck = CheckAngle(currentAngle, startAngle);

                            if (angleCheck < angleTolerance)
                            {
                                return;
                            }
                            else
                            {
                                RotateDialAntiClockwise();
                                startAngle = currentAngle;
                            }
                        }
                        else if (startAngle > currentAngle) //going clockwise;
                        {
                            angleCheck = CheckAngle(currentAngle, startAngle);

                            if (angleCheck < angleTolerance)
                            {
                                return;
                            }
                            else
                            {
                                RotateDialClockwise();
                                startAngle = currentAngle;
                            }
                        }
                    }
                    else
                    {
                        if (startAngle < currentAngle)//clockwise
                        {
                            RotateDialClockwise();
                            startAngle = currentAngle;
                        }
                        else if (startAngle > currentAngle)
                        {
                            RotateDialAntiClockwise();
                            startAngle = currentAngle;
                        }
                    }
                }
            }
            else
            {
                requiresStartAngle = false;
                startAngle = currentAngle;
            }
        }

        private float CheckAngle(float currentAngle, float startAngle)
        {
            var checkAngleTravelled = (360f - currentAngle) + startAngle;
            return (checkAngleTravelled);
        }

        private void RotateDialClockwise()
        {
            linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y - snapRotationAmout, linkedDial.localEulerAngles.z);
            GetComponent<IDial>().DialChanged(linkedDial.localEulerAngles.y);
        }

        private void RotateDialAntiClockwise()
        {
            linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y + snapRotationAmout, linkedDial.localEulerAngles.z);
            GetComponent<IDial>().DialChanged(linkedDial.localEulerAngles.y);
        }
    }
}
