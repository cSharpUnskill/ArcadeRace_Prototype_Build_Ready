using UnityEngine;

namespace Cars
{
    public class CheckpointComponent : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) => ScenarioSwitcher.OnEvent(PlayerAction.Checkpoint);
    }
}