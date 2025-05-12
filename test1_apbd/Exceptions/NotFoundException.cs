namespace test1_apbd.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        /* Implement this function */
        public NotFoundException(string? message) : base(message)
        {
        }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}