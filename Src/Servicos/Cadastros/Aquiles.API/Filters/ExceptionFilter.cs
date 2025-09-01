using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Aquiles.Exception.AquilesException;
using Aquiles.Communication.Responses;

namespace Aquiles.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is AquilesException)
            HandleHermesExceptions(context);
        else
            ThrowUndefinedError(context);
    }

    private static void HandleHermesExceptions(ExceptionContext context)
    {
        if (context.Exception is ValidationErrorException)
            ThrowValidationErrorException(context);
        else if (context.Exception is InvalidLoginException)
            ThrowLoginException(context);
    }

    private static void ThrowLoginException(ExceptionContext context)
    {
        var erroLogin = context.Exception as InvalidLoginException;
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(new ResponseErrorJson(erroLogin.Message));
    }

    private static void ThrowValidationErrorException(ExceptionContext context)
    {
        var validationErrorException = context.Exception as ValidationErrorException;
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ResponseErrorJson(validationErrorException.Errors));
    }

    private static void ThrowUndefinedError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson("Erro desconhecido"));
    }
}
