namespace RestaurantAPI.Excepctions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
        
    }
}