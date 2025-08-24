namespace Aquiles.Communication.Responses;
public class ResponseErrorJson
{
    public IList<string> Messages { get; set; }
    public ResponseErrorJson(string messages)
    {
        Messages = new List<string>() {
            messages
        };
    }

    public ResponseErrorJson(IList<string> messages)
    {
        Messages = messages;
    }
}
