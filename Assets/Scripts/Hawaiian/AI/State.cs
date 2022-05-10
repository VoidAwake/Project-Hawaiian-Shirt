using UnityEngine;

namespace Hawaiian.AI
{
    [CreateAssetMenu (menuName = "PluggableAI/State")]
    public class State : ScriptableObject
    {
        public Action[] actions;
        public Transition[] transitions;

        public void UpdateState(EnemyFSM fsm)
        {
            DoActions(fsm);
            CheckTransitions(fsm);
        }

        private void DoActions(EnemyFSM fsm)
        {
            foreach (var action in actions)
            {
                action.Act(fsm);
            }
        }

        private void CheckTransitions(EnemyFSM fsm)
        {
            foreach (var transition in transitions)
            {
                bool decisionSucceeded = transition.decision.Decide(fsm);

                if (decisionSucceeded)
                {
                    fsm.TransitionToState(transition.trueState);
                }
                else
                {
                    fsm.TransitionToState(transition.falseState);
                }
            }
        }
    }
}