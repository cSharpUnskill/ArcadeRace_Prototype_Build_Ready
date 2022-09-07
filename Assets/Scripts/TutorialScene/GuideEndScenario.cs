namespace Cars
{
    public class GuideEndScenario : Scenario
    {
        public GuideEndScenario(ScenarioSwitcher switcher)
        {
            Switcher = switcher;
            Delay = Switcher.Delay;
        }

        public override void Exec()
        {
            Switcher.TextField.SetText("guideEnd", Switcher.FieldHighlight);
        }

        public override void Update()
        {
        }
    }
}