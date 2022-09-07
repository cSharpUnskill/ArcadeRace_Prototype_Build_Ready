namespace Cars
{
    public class MiddleMouseButtonScenario : Scenario
    {
        public MiddleMouseButtonScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideMiddleButton", Switcher.FieldHighlight);
        }

        public override void Update()
        {
            if (IsDelayOver() && ScenarioSwitcher.CurrentEvent == PlayerAction.AButton)
                Switcher.SwitchScenario<AButtonScenario>();
        }
    }
}