using System;

namespace API.Errors;

public class ApiErrorClass(int status,string message,string? details)
{
    public int Status { get; set; } = status;

    public string Message { get; set; } = message;

    public string? Detials { get; set; } = details;
}
