namespace Cars
{
    public class SButtonScenario : Scenario
    {
        public SButtonScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideSButton", Switcher.FieldHighlight);
            Switcher.CarComponent.SetCheckingAccelerationState(true);
        }

        public override void Update()
        {
            if (IsDelayOver() && ScenarioSwitcher.CurrentEvent == PlayerAction.SpaceButton)
                Switcher.SwitchScenario<SpaceButtonScenario>(10f);
        }
    }
}