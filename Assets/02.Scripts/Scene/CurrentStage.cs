using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StageType
{
    Stage1,
    Stage2,
    Stage3
}

public class CurrentStage : MonoBehaviour
{
    public StageType StageType;
}
