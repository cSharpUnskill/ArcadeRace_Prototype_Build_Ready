using UnityEngine;

namespace Cars
{
    public class SpaceButtonScenario : Scenario
    {
        public SpaceButtonScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideSpaceButton", Switcher.FieldHighlight);
            Switcher.CarComponent.SetCheckingAccelerationState(false);
            Switcher.Checkpoint.gameObject.SetActive(true);
        }

        public override void Update()
        {
            if (IsDelayOver() && ScenarioSwitcher.CurrentEvent == PlayerAction.WButton)
                Switcher.SwitchScenario<WButtonScenario>();
        }
    }
}