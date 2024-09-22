namespace WebAPI.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) 
        {
        }

        public ValidationException(string message, IEnumerable<string> errors) : 
            base($"{message}: {string.Join(", ", errors)}") 
        {
        }
    }
}
