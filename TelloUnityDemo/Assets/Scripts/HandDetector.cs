using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using TMPro;
using TelloLib;

public class HandDetector : MonoBehaviour
{
    /*
    public TelloController telloController;
    public Handedness handedness;
    public float palmDirectionThreshold = 0.8f;
    //hand stay
    public float stayingThreshold = 0.01f; //threshold: 可容忍的誤差，1表示最不精準
    public float stayTime = 0.25f;
    public float stayTimeBefore = 0.5f;
    private Vector3 lastHandPosition;
    private float elapsedStayTime = 0.0f;
    private bool isHandStaying = false;
    private bool handStayed;
    //private bool handStayedMovement;

    //fingers straight
    public float alignmentThreshold = 0.09f;

    //fist
    public float[] fingerThresholds = new float[] { 0.08f, 0.08f, 0.08f, 0.08f, 0.08f };
    public float fistPalmDirectionThreshold = 0.1f;

    //index point
    public float IndexThreshold = 0.8f;

    //檢查是否起飛
    //public bool hasTakenOff = false;

    //flying feedback
    public string pose_string;
    public string distance_string;
    public string altitude_string;
    public string battery_string;
    public TMP_Text fly_distance;
    public TMP_Text fly_altitude;
    public TMP_Text posture;
    public TMP_Text batteryStatus;


    void Update()
    {
        handStayed = CheckHandStaying(handedness, stayingThreshold, stayTime);
        //handStayedMovement = CheckHandStaying(handedness, stayingThreshold, stayTimeBefore);
        posture.text = pose_string;
        fly_distance.text = distance_string;
        // 將累加的rx, ry數據傳到fly_distance文本
        distance_string = $"Distance : X: {Tello.state.posX}, Y: {Tello.state.posY}";

        //回傳高度到Altitude
        fly_altitude.text = altitude_string;
        altitude_string = $"Altitude : {Tello.state.height}";

        //回傳電量到battery
        batteryStatus.text = battery_string;
        battery_string = $"Battery : {Tello.state.batteryPercentage}";

        //往上
        if (PalmFacingDirection(handedness, Vector3.up, palmDirectionThreshold) && AreFingersStraight(handedness))
        {
            if (handStayed)
            {
                pose_string = "Posture : Go up";
                telloController.OnMoveUpPress();
                //hasTakenOff = true;
            }
        }

        //往下
        else if (PalmFacingDirection(handedness, Vector3.down, palmDirectionThreshold) && AreFingersStraight(handedness))
        {
            if (handStayed)
            {
                pose_string = "Posture : Go down";
                telloController.OnMoveDownPress();
                //hasTakenOff = true;
            }
        }

        //往左
        else if (PalmFacingDirection(handedness, Vector3.left, palmDirectionThreshold) && AreFingersStraight(handedness))
        {
            if (handStayed)
            {
                pose_string = "Posture : Go left";
                telloController.OnMoveLeftPress();
                //hasTakenOff = true;
            }
        }

        //往右
        else if (PalmFacingDirection(handedness, Vector3.right, palmDirectionThreshold) && AreFingersStraight(handedness))
        {
            if (handStayed)
            {
                pose_string = "Posture : Go right";
                telloController.OnMoveRightPress();
                //hasTakenOff = true;
            }
        }

        //往後
        else if (PalmFacingDirection(handedness, Vector3.back, palmDirectionThreshold) && AreFingersStraight(handedness))
        {
            pose_string = "Posture : Go backward";
            if (handStayed)
            {
                telloController.OnMoveBackwardPress();
                //hasTakenOff = true;
            }
        }

        //往前
        else if (PalmFacingDirection(handedness, Vector3.forward, palmDirectionThreshold) && AreFingersStraight(handedness))
        {
            if (handStayed)
            {
                pose_string = "Posture : Go forward";
                telloController.OnMoveForwardPress();
                //hasTakenOff = true;
            }
        }

        //懸停
        else if (IsFourFingerCurled(handedness) && PalmFacingDirection(handedness, Vector3.up, fistPalmDirectionThreshold))
        {
            if (handStayed)
            {
                pose_string = "Posture : Stop";
                telloController.ClearAxisValues();
                //hasTakenOff = true;
            }
        }

        //起飛
        else if (IsThreeFingerCurled(handedness)
            && IsIndexStraightAndDirection(handedness, TrackedHandJoint.IndexMetacarpal, TrackedHandJoint.IndexKnuckle, TrackedHandJoint.IndexMiddleJoint, TrackedHandJoint.IndexDistalJoint, TrackedHandJoint.IndexTip) == 1)
        {
            if (handStayed)
            {
                pose_string = "Posture : Take off";
                telloController.Takeoff();
                //hasTakenOff = true;

            }
        }

        //降落
        else if (IsThreeFingerCurled(handedness)
        && IsIndexStraightAndDirection(handedness, TrackedHandJoint.IndexMetacarpal, TrackedHandJoint.IndexKnuckle, TrackedHandJoint.IndexMiddleJoint, TrackedHandJoint.IndexDistalJoint, TrackedHandJoint.IndexTip) == 2)
        {
            if (handStayed)
            {
                pose_string = "Posture : Land";
                telloController.Land();
                //hasTakenOff = false;
            }
        }

        //左旋轉
        else if (IsThreeFingerCurled(handedness)
        && IsIndexStraightAndDirection(handedness, TrackedHandJoint.IndexMetacarpal, TrackedHandJoint.IndexKnuckle, TrackedHandJoint.IndexMiddleJoint, TrackedHandJoint.IndexDistalJoint, TrackedHandJoint.IndexTip) == 3)
        {
            if (handStayed)
            {
                pose_string = "Posture : Rotate left";
                telloController.OnTurnLeftPress();
                //hasTakenOff = true;
            }
        }

        //右旋轉
        else if (IsThreeFingerCurled(handedness)
                && IsIndexStraightAndDirection(handedness, TrackedHandJoint.IndexMetacarpal, TrackedHandJoint.IndexKnuckle, TrackedHandJoint.IndexMiddleJoint, TrackedHandJoint.IndexDistalJoint, TrackedHandJoint.IndexTip) == 4)
        {
            if (handStayed)
            {
                pose_string = "Posture : Rotate right";
                telloController.OnTurnRightPress();
                //hasTakenOff = true;
            }
        }
    }

    private bool PalmFacingDirection(Handedness handedness, Vector3 direction, float threshold)
    {
        MixedRealityPose palmPose;
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, handedness, out palmPose))
        {
            Vector3 palmUp = -palmPose.Up;
            return Vector3.Dot(palmUp, direction) > threshold;
        }
        return false;
    }


    private bool AreFingersStraight(Handedness handedness)
    {
        return IsFingerStraight(handedness, TrackedHandJoint.IndexMetacarpal, TrackedHandJoint.IndexKnuckle, TrackedHandJoint.IndexMiddleJoint, TrackedHandJoint.IndexDistalJoint, TrackedHandJoint.IndexTip) &&
               IsFingerStraight(handedness, TrackedHandJoint.MiddleMetacarpal, TrackedHandJoint.MiddleKnuckle, TrackedHandJoint.MiddleMiddleJoint, TrackedHandJoint.MiddleDistalJoint, TrackedHandJoint.MiddleTip) &&
               IsFingerStraight(handedness, TrackedHandJoint.RingMetacarpal, TrackedHandJoint.RingKnuckle, TrackedHandJoint.RingMiddleJoint, TrackedHandJoint.RingDistalJoint, TrackedHandJoint.RingTip) &&
               IsFingerStraight(handedness, TrackedHandJoint.PinkyMetacarpal, TrackedHandJoint.PinkyKnuckle, TrackedHandJoint.PinkyMiddleJoint, TrackedHandJoint.PinkyDistalJoint, TrackedHandJoint.PinkyTip);
    }

    private bool IsFingerStraight(Handedness handedness, TrackedHandJoint metacarpal, TrackedHandJoint knuckle, TrackedHandJoint middle, TrackedHandJoint distal, TrackedHandJoint tip)
    {
        MixedRealityPose metacarpalPose, knucklePose, middlePose, distalPose, tipPose;
        bool metacarpalAvailable = HandJointUtils.TryGetJointPose(metacarpal, handedness, out metacarpalPose);
        bool knuckleAvailable = HandJointUtils.TryGetJointPose(knuckle, handedness, out knucklePose);
        bool middleAvailable = HandJointUtils.TryGetJointPose(middle, handedness, out middlePose);
        bool distalAvailable = HandJointUtils.TryGetJointPose(distal, handedness, out distalPose);
        bool tipAvailable = HandJointUtils.TryGetJointPose(tip, handedness, out tipPose);

        if (metacarpalAvailable && knuckleAvailable && middleAvailable && distalAvailable && tipAvailable)
        {
            Vector3 knuckleDirection = (knucklePose.Position - metacarpalPose.Position).normalized;
            Vector3 middleDirection = (middlePose.Position - knucklePose.Position).normalized;
            Vector3 distalDirection = (distalPose.Position - middlePose.Position).normalized;
            Vector3 tipDirection = (tipPose.Position - distalPose.Position).normalized;

            return Vector3.Dot(knuckleDirection, middleDirection) > alignmentThreshold &&
                   Vector3.Dot(middleDirection, distalDirection) > alignmentThreshold &&
                   Vector3.Dot(distalDirection, tipDirection) > alignmentThreshold;
        }
        return false;
    }


    private bool CheckHandStaying(Handedness handedness, float positionThreshold, float stayTime)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, handedness, out MixedRealityPose pose))
        {
            Vector3 currentHandPosition = pose.Position;

            if (isHandStaying)
            {
                // Check if the hand has moved beyond the threshold
                if (Vector3.Distance(lastHandPosition, currentHandPosition) > positionThreshold)
                {
                    // Reset the timer if the hand moved
                    elapsedStayTime = 0.0f;
                    isHandStaying = false;
                }
                else
                {
                    // Increment the stay time
                    elapsedStayTime += Time.deltaTime;
                    if (elapsedStayTime >= stayTime)
                    {
                        // Hand has stayed in the same position for the required time
                        return true;
                    }
                }
            }
            else
            {
                // Start tracking the hand position
                lastHandPosition = currentHandPosition;
                elapsedStayTime = 0.0f;
                isHandStaying = true;
            }
        }
        else
        {
            // Hand is not tracked, reset state
            isHandStaying = false;
            elapsedStayTime = 0.0f;
        }

        return false;
    }


    private enum FingerIndex
    {
        Thumb = 0,
        Index,
        Middle,
        Ring,
        Pinky
    }

    private bool IsOneFingerCurled(TrackedHandJoint tipJoint, TrackedHandJoint knuckleJoint, Handedness handedness, string fingerName, float threshold)
    {
        if (HandJointUtils.TryGetJointPose(tipJoint, handedness, out MixedRealityPose tipPose) &&
            HandJointUtils.TryGetJointPose(knuckleJoint, handedness, out MixedRealityPose knucklePose))
        {
            float distance = Vector3.Distance(tipPose.Position, knucklePose.Position);

            return distance < threshold;
        }

        return false;
    }

    private bool IsFourFingerCurled(Handedness handedness)
    {
        bool isIndexCurled = IsOneFingerCurled(TrackedHandJoint.IndexTip, TrackedHandJoint.IndexKnuckle, handedness, "Index", fingerThresholds[(int)FingerIndex.Index]);
        bool isMiddleCurled = IsOneFingerCurled(TrackedHandJoint.MiddleTip, TrackedHandJoint.MiddleKnuckle, handedness, "Middle", fingerThresholds[(int)FingerIndex.Middle]);
        bool isRingCurled = IsOneFingerCurled(TrackedHandJoint.RingTip, TrackedHandJoint.RingKnuckle, handedness, "Ring", fingerThresholds[(int)FingerIndex.Ring]);
        bool isPinkyCurled = IsOneFingerCurled(TrackedHandJoint.PinkyTip, TrackedHandJoint.PinkyKnuckle, handedness, "Pinky", fingerThresholds[(int)FingerIndex.Pinky]);

        Debug.Log($"ThumbsUp: Index: {isIndexCurled}, Middle: {isMiddleCurled}, Ring: {isRingCurled}, Pinky: {isPinkyCurled}");

        return isIndexCurled && isMiddleCurled && isRingCurled && isPinkyCurled;
    }

    private bool IsThreeFingerCurled(Handedness handedness)
    {
        bool isMiddleCurled = IsOneFingerCurled(TrackedHandJoint.MiddleTip, TrackedHandJoint.MiddleKnuckle, handedness, "Middle", fingerThresholds[(int)FingerIndex.Middle]);
        bool isRingCurled = IsOneFingerCurled(TrackedHandJoint.RingTip, TrackedHandJoint.RingKnuckle, handedness, "Ring", fingerThresholds[(int)FingerIndex.Ring]);
        bool isPinkyCurled = IsOneFingerCurled(TrackedHandJoint.PinkyTip, TrackedHandJoint.PinkyKnuckle, handedness, "Pinky", fingerThresholds[(int)FingerIndex.Pinky]);

        Debug.Log($"IndexUp: Middle: {isMiddleCurled}, Ring: {isRingCurled}, Pinky: {isPinkyCurled}");

        return isMiddleCurled && isRingCurled && isPinkyCurled;
    }

    private int IsIndexStraightAndDirection(Handedness handedness, TrackedHandJoint metacarpal, TrackedHandJoint knuckle, TrackedHandJoint middle, TrackedHandJoint distal, TrackedHandJoint tip)
    {
        MixedRealityPose metacarpalPose, knucklePose, middlePose, distalPose, tipPose;
        bool metacarpalAvailable = HandJointUtils.TryGetJointPose(metacarpal, handedness, out metacarpalPose);
        bool knuckleAvailable = HandJointUtils.TryGetJointPose(knuckle, handedness, out knucklePose);
        bool middleAvailable = HandJointUtils.TryGetJointPose(middle, handedness, out middlePose);
        bool distalAvailable = HandJointUtils.TryGetJointPose(distal, handedness, out distalPose);
        bool tipAvailable = HandJointUtils.TryGetJointPose(tip, handedness, out tipPose);

        if (metacarpalAvailable && middleAvailable && knuckleAvailable && distalAvailable && tipAvailable)
        {
            Debug.Log("Joint poses available");

            Vector3 knuckleDirection = (knucklePose.Position - metacarpalPose.Position).normalized;
            Vector3 middleDirection = (middlePose.Position - knucklePose.Position).normalized;
            Vector3 distalDirection = (distalPose.Position - middlePose.Position).normalized;
            Vector3 tipDirection = (tipPose.Position - distalPose.Position).normalized;

            bool aligned = Vector3.Dot(knuckleDirection, middleDirection) > alignmentThreshold &&
                   Vector3.Dot(middleDirection, distalDirection) > alignmentThreshold &&
                   Vector3.Dot(distalDirection, tipDirection) > alignmentThreshold;

            Debug.Log($"Alignment: {Vector3.Dot(distalDirection, tipDirection)}");

            if (aligned)
            {
                Vector3 indexDirection = knuckleDirection + middleDirection + distalDirection + tipDirection;
                Debug.Log($"IndexDirection: {indexDirection}");

                if (Vector3.Dot(indexDirection, Vector3.up) > IndexThreshold)
                    return 1;
                else if (Vector3.Dot(indexDirection, -Vector3.up) > IndexThreshold)
                    return 2;
                else if (Vector3.Dot(indexDirection, Vector3.left) > IndexThreshold)
                    return 3;
                else if (Vector3.Dot(indexDirection, Vector3.right) > IndexThreshold)
                    return 4;
            }
        }

        return 0;
    }*/
}