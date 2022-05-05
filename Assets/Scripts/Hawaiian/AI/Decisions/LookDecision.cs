namespace Hawaiian.AI.Decisions
{
    public class LookDecision : Decision
    {
        public override bool Decide(EnemyFSM fsm)
        {
            bool targetVisible = Look(fsm);
            return targetVisible;
        }

        public bool Look(EnemyFSM fsm)
        {
            return fsm;
        }
    }
}
