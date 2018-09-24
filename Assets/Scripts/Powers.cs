using UnityEngine;
using System.Collections;

public class Powers{

    public static bool ShieldActivated;
    public static bool MagnetActivated;
    public static bool BounceActivated;
    public static bool DangerActivated;

    public static void Reset()
    {
        ShieldActivated =
            MagnetActivated =
            BounceActivated =
             DangerActivated = false;
    }

    public override string ToString()
    {
        return string.Format(
            "(Powers: s:{0} m:{1} b:{2} d:{3})",
            ShieldActivated,
            MagnetActivated,
            BounceActivated,
            DangerActivated);
    }

}
