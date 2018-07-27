namespace Alpha.Transfer.Tests.Helpers
{
    using System;

    public static class ExceptionRecorder
    {
        public static Exception Record(Action action)
        {
            // Act
            Exception actualException = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                actualException = ex;
            }

            return actualException;
        }
    }
}
