using System.ComponentModel;
using System.Reflection;

namespace Domain.Enums;

public enum ErrorCodigo
{
    [Description("Excepcion no controlada.")]
    ExcepcionNoControlada = -1,

    [Description("Error desconocido.")]
    ErrorDesconocido = 0,

    [Description("El nombre de usuario ya existe en el sistema.")]
    NombreDeUsuarioRepetido = 1,

    [Description("El correo electrónico ya está registrado en el sistema.")]
    CorreoRepetido = 2,

    [Description("El sistema no logró generar un correo electrónico correctamente.")]
    ErrorAlGenerarCorreo = 3,

    [Description("Ocurrio un error al intentar llamar a un procedimiento almacenado del sistema.")]
    ErrorProcedimientoAlmacenado = 4,

    [Description("No existe una cuenta registrada con ese correo.")]
    CorreoNoEncontrado = 5,

    [Description("La contraseña es incorrecta.")]
    ContrasenaIncorrecta = 6,

    [Description("El correo no ha sido verificado.")]
    CorreoNoVerificado = 7,

    [Description("El correo ya está verificado.")]
    CorreoYaVerificado = 8,

    [Description("El código de verificación no es válido.")]
    CodigoVerificacionInvalido = 9,

    [Description("El código de verificación ha expirado.")]
    CodigoVerificacionExpirado = 10,

    [Description("No se encontró una sesión activa para el GUID proporcionado.")]
    SesionNoValida = 11,

    [Description("El usuario ya tiene un embarazo activo registrado.")]
    EmbarazoActivoPrexistente = 12,

    [Description("Ocurrió una inconsistencia con una fecha digitada.")]
    FechaInvalida = 13,

    [Description("No se encontró un embarazo activo con el ID proporcionado.")]
    EmbarazoValidoNoEncontrado = 14,

    [Description("No se encontró un hospital con el ID proporcionado.")]
    HospitalNoEncontrado = 15,

    [Description("No se encontró un embarazo activo para el usuario.")]
    EmbarazoActivoNoEncontrado = 16,

    [Description("El estado proporcionado no es válido.")]
    EstadoInvalido = 17,

    [Description("No se encontró una cita con el ID proporcionado.")]
    CitaNoEncontrada = 18,

    [Description("No se encontró un síntoma con el ID proporcionado.")]
    SintomaNoEncontrado = 19,

    [Description("Datos requeridos no ingresados o inválidos.")]
    DatosInvalidos = 20,

    [Description("Gravedad de eventualidad ingresada inválida.")]
    GravedadEventualidadInvalida = 21,

    [Description("Intensidad de contracción ingresada inválida.")]
    IntensidadContraccionInvalida = 22,

    [Description("No hay un código de verificación activo para este usuario.")]
    CodigoVerificacionNoEncontrado = 23,

    [Description("El usuario asociado a la sesión no existe..")]
    UsuarioNoEncontrado = 24,
}

public static class ErrorCodigoExtensions
{
    public static ErrorCodigo ObtenerCodigoErrorEnum(int? codigoError)
    {
        if (codigoError == null)
        {
            // Si el código de error es nulo, puedes devolver un valor por defecto o lanzar una excepción
            return ErrorCodigo.ErrorDesconocido;
        }

        // Intenta convertir el código de error en un valor del enum
        if (Enum.IsDefined(typeof(ErrorCodigo), codigoError))
        {
            return (ErrorCodigo)codigoError;
        }
        else
        {
            // Si no es un valor definido en el enum, puedes manejarlo de alguna forma, como devolver un error desconocido
            return ErrorCodigo.ErrorDesconocido;
        }
    }


    public static string GetDescription(this Enum value)
    {
        // Obtener el campo del enum
        FieldInfo field = value.GetType().GetField(value.ToString());

        // Si no se encuentra el campo, devolver el nombre estándar
        if (field == null) return value.ToString();

        // Obtener el atributo Description
        DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

        // Si se encuentra el atributo Description, devolver su valor; si no, devolver el nombre estándar
        return attribute == null ? value.ToString() : attribute.Description;
    }
}
