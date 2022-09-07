namespace Cars
{
    public class WelcomeScenario : Scenario
    {
        public WelcomeScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideWelcome", Switcher.FieldShowUp);
            Switcher.PlayerInput.DisableAcceleration();
        }

        public override void Update()
        {
            if (IsDelayOver() && ScenarioSwitcher.CurrentEvent == PlayerAction.MiddleMouseButton)
                Switcher.SwitchScenario<MiddleMouseButtonScenario>();
        }
    }
}