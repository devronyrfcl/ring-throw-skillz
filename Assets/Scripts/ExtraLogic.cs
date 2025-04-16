using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillzSDK;

public class ExtraLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnMatchWillBeginExtraLogic(Match match)
    {
        Debug.Log("Begining Match...");
    }
}
