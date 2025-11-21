using Normal.Realtime;
using UnityEngine;

[RealtimeModel]
public partial class GateControllerSyncModel
{
    [RealtimeProperty(1, true, true)]
    private bool _isMoving;
}