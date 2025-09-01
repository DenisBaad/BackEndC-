using Aquiles.Utils.Services;

namespace CommonTestUtilities.Token;
public class TokenControllerBuilder
{
    public static TokenController Build()
    {
        return new TokenController("O7c6ruB2TUSxIXYmRUlFSnIkNnFHRTjkMNRSOW1lMjJlBBC=", 1000);
    }
}
