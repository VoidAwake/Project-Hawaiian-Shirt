using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        for(int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(fsm);
        }
    }

    private void CheckTransitions(EnemyFSM fsm)
    {
        for(int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(fsm);

            if (decisionSucceeded)
            {
                fsm.TransitionToState(transitions[i].trueState);
            }
            else
            {
                fsm.TransitionToState(transitions[i].falseState);
            }
        }
    }
}