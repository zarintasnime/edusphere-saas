namespace AssignmentSubmissionManagementSystem.Application.Common.Exceptions;

/// <summary>
/// Base type for expected (non-bug) application errors.
/// The API exception middleware maps <see cref="StatusCode"/> straight onto the HTTP response,
/// so the React client always receives { message, statusCode, errors } instead of an opaque 500.
/// </summary>
public abstract class AppException : Exception
{
    protected AppException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}


/// <summary>400 - request understood, but the data is not usable.</summary>
public sealed class BadRequestException : AppException
{
    public BadRequestException(string message) : base(message, 400) { }
}


/// <summary>401 - bad credentials, or a missing / expired token.</summary>
public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Invalid email or password.")
        : base(message, 401) { }
}


/// <summary>403 - authenticated, but not allowed to touch this resource.</summary>
public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string message = "You are not allowed to perform this action.")
        : base(message, 403) { }
}


/// <summary>404 - the requested resource does not exist.</summary>
public sealed class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, 404) { }

    public NotFoundException(string entityName, object key)
        : base($"{entityName} with id '{key}' was not found.", 404) { }
}


/// <summary>409 - conflicts with existing state (duplicate email, duplicate code, ...).</summary>
public sealed class ConflictException : AppException
{
    public ConflictException(string message) : base(message, 409) { }
}


/// <summary>422 - the payload failed FluentValidation. Carries the per-field errors.</summary>
public sealed class ValidationFailedException : AppException
{
    public ValidationFailedException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.", 422)
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; }
}
