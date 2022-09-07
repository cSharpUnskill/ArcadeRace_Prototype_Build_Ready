namespace Cars
{
    public class CheckpointReachedScenario : Scenario
    {
        public CheckpointReachedScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideCheckpoint", Switcher.FieldHighlight);
            Switcher.CarComponent.RemoteHandBrake();
            Switcher.Speedometer.gameObject.SetActive(false);
        }

        public override void Update()
        {
            if (IsDelayOver() && ScenarioSwitcher.CurrentEvent == PlayerAction.EscButton)
                Switcher.SwitchScenario<EscButtonScenario>();
        }
    }
}