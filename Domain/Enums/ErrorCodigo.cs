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
    ErrorAlGenerarCorreo = 3
}

public static class ErrorCodigoExtensions
{
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
