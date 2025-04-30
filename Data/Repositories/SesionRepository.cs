using Data.Contexts;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Data.Repositories
{
    public class SesionRepository : ISesionRepository
    {
        private readonly BumpContext _context;

        public SesionRepository(BumpContext context)
        {
            _context = context;
        }




        public async Task<(bool Success, string NombreUsuario, bool CorreoVerificado, Guid SessionGuid, int? CodigoError, string DetalleError, string DetalleUsuario)> LoginUsuarioAsync(string correo, string contrasena)
        {
            var query = "EXEC SP_LOGIN_USUARIO @CORREO, @CONTRASENA, @RESULTADO OUTPUT, @NOMBRE_USUARIO OUTPUT, @CORREO_VERIFICADO OUTPUT, @SESSION_GUID OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter("@CORREO", correo));
                    command.Parameters.Add(new SqlParameter("@CONTRASENA", contrasena));

                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var nombreUsuarioParam = new SqlParameter("@NOMBRE_USUARIO", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    var correoVerificadoParam = new SqlParameter("@CORREO_VERIFICADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var sessionGuidParam = new SqlParameter("@SESSION_GUID", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output };
                    var codigoErrorParam = new SqlParameter("@CODIGO_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var detalleErrorParam = new SqlParameter("@DETALLE_ERROR", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    var detalleUsuarioParam = new SqlParameter("@DETALLE_USUARIO", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultadoParam);
                    command.Parameters.Add(nombreUsuarioParam);
                    command.Parameters.Add(correoVerificadoParam);
                    command.Parameters.Add(sessionGuidParam);
                    command.Parameters.Add(codigoErrorParam);
                    command.Parameters.Add(detalleErrorParam);
                    command.Parameters.Add(detalleUsuarioParam);

                    await command.ExecuteNonQueryAsync();

                    bool success = (bool)resultadoParam.Value;
                    string nombreUsuario = nombreUsuarioParam.Value as string ?? string.Empty;
                    bool correoVerificado = correoVerificadoParam.Value as bool? ?? false;
                    Guid sessionGuid = sessionGuidParam.Value as Guid? ?? Guid.Empty;
                    int? codigoError = codigoErrorParam.Value as int?;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;

                    return (success, nombreUsuario, correoVerificado, sessionGuid, codigoError, detalleError, detalleUsuario);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al ejecutar SP_LOGIN_USUARIO: {ex.Message}", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }



        public async Task<(bool success, string detalleError)> ValidarSesionAsync(Guid sessionGuid)
        {
            var query = "EXEC SP_VALIDAR_SESION @SESSION_GUID, @RESULTADO OUTPUT, @DETALLE_ERROR OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter("@SESSION_GUID", sessionGuid));

                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var detalleErrorParam = new SqlParameter("@DETALLE_ERROR", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultadoParam);
                    command.Parameters.Add(detalleErrorParam);

                    await command.ExecuteNonQueryAsync();

                    bool success = (bool)resultadoParam.Value;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;

                    return (success, detalleError);
                }
            }
            catch (SqlException ex)
            {
                return (false, $"Error al ejecutar SP_VALIDAR_SESION: {ex.Message}");
            }
            finally
            {
                await connection.CloseAsync();
            }
        }






































    }
}

