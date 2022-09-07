using UnityEngine;

namespace Cars
{
    public abstract class Scenario
    {
        protected float Delay;
        protected ScenarioSwitcher Switcher;
        public abstract void Exec();
        public abstract void Update();
        public void SetExtraDelay(float extraDelay) => Delay += extraDelay;
        protected bool IsDelayOver()
        {
            Delay -= Time.unscaledDeltaTime;
            return Delay <= 0;
        }
    }
}