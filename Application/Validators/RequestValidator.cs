using PortaldeCompras.Application.DTOs;
using PortaldeCompras.Application.Common;

namespace PortaldeCompras.Application.Validators
{
    public static class RequestValidator
    {
        public static ValidationResult Validate(RequestDto dto)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(dto.Url))
                result.AddError("A URL não pode estar vazia.");

            else if (!Uri.IsWellFormedUriString(dto.Url, UriKind.Absolute))
                result.AddError("A URL informada não é válida.");

            if (string.IsNullOrWhiteSpace(dto.OutputPath))
                result.AddError("O caminho de saída (OutputPath) é obrigatório.");

            return result;

        }
    }
}
