using UnityEngine;

namespace Cars
{
    public class WButtonScenario : Scenario
    {
        public WButtonScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideWButton",Switcher.FieldHighlight);
            Switcher.Speedometer.alpha = 1;
        }

        public override void Update()
        {
            if (IsDelayOver() && ScenarioSwitcher.CurrentEvent == PlayerAction.Checkpoint)
                Switcher.SwitchScenario<CheckpointReachedScenario>();
        }
    }
}