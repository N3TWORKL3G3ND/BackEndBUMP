using Domain.Enums;


namespace Domain.Exceptions
{
    public class InternalApplicationException : Exception
    {
        public ErrorCodigo ErrorCodigo { get; }
        public int ErrorCodigoValor => (int)ErrorCodigo;

        public InternalApplicationException(ErrorCodigo errorCodigo, string message = null)
            : base(message ?? errorCodigo.GetDescription())
        {
            ErrorCodigo = errorCodigo;
        }

        public InternalApplicationException(ErrorCodigo errorCodigo, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCodigo = errorCodigo;
        }
    }
}



/*
 EJEMPLO DE USO

public async Task<EmbarazoListResponse> ListarEmbarazosAsync(ListarEmbarazosRequest request)
    {
        // Validación
        if (request.IdUsuario <= 0)
            throw new InternalApplicationException(ErrorCodigo.DatosEmbarazoInvalidos, "El ID de usuario debe ser mayor que cero.");

        try
        {
            string resultado = await _repository.ListarEmbarazosAsync(request.IdUsuario);
            if (string.IsNullOrEmpty(resultado))
                throw new InternalApplicationException(ErrorCodigo.UsuarioNoEncontrado, "No se encontraron embarazos para este usuario.");

            return new EmbarazoListResponse { Resultado = resultado };
        }
        catch (Exception ex)
        {
            // Envolver excepciones del repositorio
            throw new InternalApplicationException(ErrorCodigo.ErrorProcedimientoAlmacenado, 
                "Error al listar embarazos: " + ex.Message, ex);
        }
    }
 */