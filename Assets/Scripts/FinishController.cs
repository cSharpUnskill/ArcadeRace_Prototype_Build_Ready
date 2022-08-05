using System.Collections;
using UnityEngine;
using System.Linq;
using TMPro;

namespace Cars
{
    public class FinishController : MonoBehaviour
    {
        [SerializeField]
        private Animator _blackScreenAnimator;
        [SerializeField]
        private LapTimer _lapTimer;
        [SerializeField]
        private PlayerInputController _player;
        [SerializeField]
        private Speedometer[] _speedometers;
        [SerializeField]
        private TextMeshProUGUI _yourTime;
        [SerializeField]
        private TextMeshProUGUI _leaderboard;
        [SerializeField]
        private TextMeshProUGUI _linePrefab;
        [SerializeField]
        private Transform _canvas;
        [SerializeField]
        private Animator _topMenuAnimator;

        private static readonly int Blackout = Animator.StringToHash("Blackout");
        private static readonly int Show = Animator.StringToHash("Show");

        void OnTriggerEnter(Collider col)
        {
            var car = col.GetComponentInParent<CarComponent>();
            if(car != null) car.RemoteHandBrake();

            Recorder.Write(_player.Name, _lapTimer.LapTime.ToString(@"mm\:ss\:f"));

            StartCoroutine(EndRaceAnimations());
            foreach (var speedometer in _speedometers)  speedometer.FinishRace();
            _lapTimer.TurnOffTimer();
            _player.FinishRace();
        }

        private IEnumerator EndRaceAnimations()
        {
            _blackScreenAnimator.SetTrigger(Blackout);

            var leaderBoard = Recorder.CurrentLeaderboard()
                .OrderBy(c => int.Parse(string.Concat(c.Value.Where(char.IsDigit))));

            yield return null;
            _yourTime.gameObject.SetActive(true);
            _yourTime.text += $" {_lapTimer.LapTime:mm\\:ss\\:f}";

            yield return new WaitForSecondsRealtime(1f);

            _leaderboard.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(1f);

            int k = 0;

            for (int i = 0; i < 7; i++)
            {
                if (leaderBoard.Count() > i)
                {
                    var line = Instantiate(_linePrefab, _canvas);
                    line.gameObject.SetActive(true);
                    line.text = $"{leaderBoard.ElementAt(i).Key} {leaderBoard.ElementAt(i).Value}";
                    line.rectTransform.anchoredPosition = new Vector2(0, 80 + k);
                    k -= 60;
                    yield return new WaitForSecondsRealtime(0.3f);
                }
            }

            _topMenuAnimator.SetTrigger(Show);
        }
    }
}
