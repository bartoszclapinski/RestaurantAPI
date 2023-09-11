namespace RestaurantAPI.Excepctions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) 
    {
        
    }
}