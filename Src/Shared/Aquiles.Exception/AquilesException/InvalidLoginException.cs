using System.Runtime.Serialization;

namespace Aquiles.Exception.AquilesException;

[Serializable]

public class InvalidLoginException : AquilesException
{
    public InvalidLoginException() : base(ResourceMensagensDeErro.LOGIN_INVALIDO) { }

    public InvalidLoginException(string message) : base(message) { }

    protected InvalidLoginException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
