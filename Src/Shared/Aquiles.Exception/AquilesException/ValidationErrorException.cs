using System.Runtime.Serialization;

namespace Aquiles.Exception.AquilesException;

[Serializable]

public class ValidationErrorException : AquilesException
{
    public IList<string> Errors { get; set; }
    public ValidationErrorException(IList<string> errors) : base(string.Empty)
    {
        Errors = errors;
    }

    protected ValidationErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
