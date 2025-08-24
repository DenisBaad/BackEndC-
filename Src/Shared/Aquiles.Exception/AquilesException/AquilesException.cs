using System.Runtime.Serialization;

namespace Aquiles.Exception.AquilesException;

[Serializable]

public class AquilesException : SystemException
{
    public AquilesException(string message) : base(message) { }

    protected AquilesException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
