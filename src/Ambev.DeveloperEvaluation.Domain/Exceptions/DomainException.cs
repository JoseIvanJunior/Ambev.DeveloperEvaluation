using System;

namespace Ambev.DeveloperEvaluation.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string? ErrorCode { get; }
        public string UserMessage { get; }

        public DomainException(string message)
            : base(message)
        {
            UserMessage = message;
        }

        public DomainException(string message, string errorCode)
            : base($"{errorCode}: {message}")
        {
            ErrorCode = errorCode;
            UserMessage = message;
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
            UserMessage = message;
        }

        public DomainException(string message, string errorCode, Exception innerException)
            : base($"{errorCode}: {message}", innerException)
        {
            ErrorCode = errorCode;
            UserMessage = message;
        }

        public static DomainException NotFound(string entityName, Guid id)
        {
            return new DomainException(
                $"{entityName} com id {id} não foi encontrado",
                "NOT_FOUND"
            );
        }

        public static DomainException InvalidOperation(string operation, string reason)
        {
            return new DomainException(
                $"Não é possível executar {operation}: {reason}",
                "INVALID_OPERATION"
            );
        }

        public static DomainException ValidationError(string field, string error)
        {
            return new DomainException(
                $"Erro de validação para o campo '{field}': {error}",
                "VALIDATION_ERROR"
            );
        }

        public static DomainException BusinessRuleViolation(string rule, string details)
        {
            return new DomainException(
                $"Violação de regra de negócios: {rule}. {details}",
                "BUSINESS_RULE_VIOLATION"
            );
        }
    }
}