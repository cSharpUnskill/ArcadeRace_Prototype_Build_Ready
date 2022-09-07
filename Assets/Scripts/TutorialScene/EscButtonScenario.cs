namespace Cars
{
    public class EscButtonScenario : Scenario
    {
        public EscButtonScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideEscButton", Switcher.FieldHighlight);
        }

        public override void Update()
        {
            if (IsDelayOver())
                Switcher.SwitchScenario<GuideEndScenario>(-1f);
        }
    }
}