namespace Cars
{
    public class DButtonScenario : Scenario
    {
        public DButtonScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideDButton", Switcher.FieldHighlight);
            Switcher.Camera.ReturnToBasePosition();
        }

        public override void Update()
        {
            if (IsDelayOver() && ScenarioSwitcher.CurrentEvent == PlayerAction.SButton)
                Switcher.SwitchScenario<SButtonScenario>();
        }
    }
}