namespace Cars
{
    public class AButtonScenario : Scenario
    {
        public AButtonScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideAButton", Switcher.FieldHighlight);
            Switcher.Camera.CheckRightWheel();
            Switcher.PlayerInput.DisableRearView();
        }

        public override void Update()
        {
            if (IsDelayOver() && ScenarioSwitcher.CurrentEvent == PlayerAction.DButton)
                Switcher.SwitchScenario<DButtonScenario>();
        }
    }
}