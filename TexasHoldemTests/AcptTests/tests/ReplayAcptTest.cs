namespace TexasHoldemTests.AcptTests.tests
{
    class ReplayAcptTest : AcptTest
    {
        protected override void SubClassInit()
        {
            //delete all games and all users, then register user1
            RestartSystem();
        }

        protected override void SubClassDispose()
        {
            //nothing to do here
        }


    }
}
