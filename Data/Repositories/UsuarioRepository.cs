using Data.Contexts;
using Domain.DTOs;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BumpContext _context;

        public UsuarioRepository(BumpContext context)
        {
            _context = context;
        }



        public async Task<(bool Success, string CodigoVerificacion, int? CodigoError, string DetalleError, string DetalleUsuario)> CrearUsuarioAsync(string nombreUsuario, string nombreCompleto, string correo, string contrasena)
        {
            var query = "EXEC SP_CREAR_USUARIO @NOMBRE_USUARIO, @NOMBRE_COMPLETO, @CORREO, @CONTRASENA, @RESULTADO OUTPUT, @CODIGO_VERIFICACION OUTPUT, @DETALLE_ERROR OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    // Parámetros de entrada
                    command.Parameters.Add(new SqlParameter("@NOMBRE_USUARIO", nombreUsuario));
                    command.Parameters.Add(new SqlParameter("@NOMBRE_COMPLETO", nombreCompleto));
                    command.Parameters.Add(new SqlParameter("@CORREO", correo));
                    command.Parameters.Add(new SqlParameter("@CONTRASENA", contrasena));

                    // Parámetros de salida
                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var codigoVerificacionParam = new SqlParameter("@CODIGO_VERIFICACION", SqlDbType.NVarChar, 10) { Direction = ParameterDirection.Output };
                    var detalleErrorParam = new SqlParameter("@DETALLE_ERROR", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    var codigoErrorParam = new SqlParameter("@CODIGO_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var detalleUsuarioParam = new SqlParameter("@DETALLE_USUARIO", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultadoParam);
                    command.Parameters.Add(codigoVerificacionParam);
                    command.Parameters.Add(detalleErrorParam);
                    command.Parameters.Add(codigoErrorParam);
                    command.Parameters.Add(detalleUsuarioParam);

                    await command.ExecuteNonQueryAsync();

                    bool success = (bool)resultadoParam.Value;
                    string codigoVerificacion = codigoVerificacionParam.Value as string ?? string.Empty;
                    int? codigoError = codigoErrorParam.Value as int?;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;


                    return (success, codigoVerificacion, codigoError, detalleError, detalleUsuario);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al ejecutar el procedimiento almacenado: " + ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }



        public async Task<(bool success, int? codigoError, string detalleError, string detalleUsuario)> ValidarCodigoVerificacionAsync(string correo, string codigoVerificacion)
        {
            var query = "EXEC SP_VALIDAR_CODIGO_VERIFICACION @CORREO, @CODIGO_VERIFICACION, @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter("@CORREO", correo));
                    command.Parameters.Add(new SqlParameter("@CODIGO_VERIFICACION", codigoVerificacion));

                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var codigoErrorParam = new SqlParameter("@CODIGO_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var detalleErrorParam = new SqlParameter("@DETALLE_ERROR", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    var detalleUsuarioParam = new SqlParameter("@DETALLE_USUARIO", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultadoParam);
                    command.Parameters.Add(codigoErrorParam);
                    command.Parameters.Add(detalleErrorParam);
                    command.Parameters.Add(detalleUsuarioParam);

                    await command.ExecuteNonQueryAsync();

                    bool success = (bool)resultadoParam.Value;
                    int? codigoError = codigoErrorParam.Value as int?;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;

                    return (success, codigoError, detalleError, detalleUsuario);
                }
            }
            catch (SqlException ex)
            {
                return (false, -1, $"Error al ejecutar SP_VALIDAR_CODIGO_VERIFICACION: {ex.Message}", "Ocurrió un error al verificar el correo.");
            }
            finally
            {
                await connection.CloseAsync();
            }
        }



        public async Task<(bool Success, string CodigoVerificacion, int? CodigoError, string DetalleError, string DetalleUsuario)> GenerarCodigoVerificacionAsync(string correo)
        {
            var query = "EXEC SP_GENERAR_CODIGO_VERIFICACION @P_CORREO, @RESULTADO OUTPUT, @CODIGO_VERIFICACION OUTPUT, @DETALLE_ERROR OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    // Parámetro de entrada
                    command.Parameters.Add(new SqlParameter("@P_CORREO", correo));

                    // Parámetros de salida
                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var codigoVerificacionParam = new SqlParameter("@CODIGO_VERIFICACION", SqlDbType.NVarChar, 10) { Direction = ParameterDirection.Output };
                    var detalleErrorParam = new SqlParameter("@DETALLE_ERROR", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    var codigoErrorParam = new SqlParameter("@CODIGO_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var detalleUsuarioParam = new SqlParameter("@DETALLE_USUARIO", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultadoParam);
                    command.Parameters.Add(codigoVerificacionParam);
                    command.Parameters.Add(detalleErrorParam);
                    command.Parameters.Add(codigoErrorParam);
                    command.Parameters.Add(detalleUsuarioParam);

                    await command.ExecuteNonQueryAsync();

                    bool success = (bool)resultadoParam.Value;
                    string codigoVerificacion = codigoVerificacionParam.Value as string ?? string.Empty;
                    int? codigoError = codigoErrorParam.Value as int?;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;

                    return (success, codigoVerificacion, codigoError, detalleError, detalleUsuario);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al ejecutar el procedimiento almacenado: " + ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RestablecerContrasenaAsync(string correo, string codigoVerificacion, string nuevaContrasena)
        {
            var query = "EXEC SP_RESTABLECER_CONTRASENA @P_CORREO, @P_CODIGO_VERIFICACION, @P_NUEVA_CONTRASENA, @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    // Parámetros de entrada
                    command.Parameters.Add(new SqlParameter("@P_CORREO", correo));
                    command.Parameters.Add(new SqlParameter("@P_CODIGO_VERIFICACION", codigoVerificacion));
                    command.Parameters.Add(new SqlParameter("@P_NUEVA_CONTRASENA", nuevaContrasena));

                    // Parámetros de salida
                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var codigoErrorParam = new SqlParameter("@CODIGO_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var detalleErrorParam = new SqlParameter("@DETALLE_ERROR", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    var detalleUsuarioParam = new SqlParameter("@DETALLE_USUARIO", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultadoParam);
                    command.Parameters.Add(codigoErrorParam);
                    command.Parameters.Add(detalleErrorParam);
                    command.Parameters.Add(detalleUsuarioParam);

                    await command.ExecuteNonQueryAsync();

                    bool success = (bool)resultadoParam.Value;
                    int? codigoError = codigoErrorParam.Value as int?;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;

                    return (success, codigoError, detalleError, detalleUsuario);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al ejecutar el procedimiento almacenado: " + ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, DatosUsuarioDto? DatosUsuario)> ObtenerDatosUsuarioAsync(Guid sessionGuid)
        {
            var query = "EXEC SP_DATOS_USUARIO @P_SESSION_GUID, @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    // Parámetro de entrada
                    command.Parameters.Add(new SqlParameter("@P_SESSION_GUID", sessionGuid));

                    // Parámetros de salida
                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var codigoErrorParam = new SqlParameter("@CODIGO_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var detalleErrorParam = new SqlParameter("@DETALLE_ERROR", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    var detalleUsuarioParam = new SqlParameter("@DETALLE_USUARIO", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultadoParam);
                    command.Parameters.Add(codigoErrorParam);
                    command.Parameters.Add(detalleErrorParam);
                    command.Parameters.Add(detalleUsuarioParam);

                    DatosUsuarioDto? datosUsuario = null;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            datosUsuario = new DatosUsuarioDto
                            {
                                NombreUsuario = reader.GetString(reader.GetOrdinal("NOMBRE_USUARIO")),
                                NombreCompleto = reader.GetString(reader.GetOrdinal("NOMBRE_COMPLETO")),
                                Correo = reader.GetString(reader.GetOrdinal("CORREO")),
                                EstadoEmbarazo = reader.GetBoolean(reader.GetOrdinal("ESTADO_EMBARAZO")),
                                SemanaEmbarazo = reader.IsDBNull(reader.GetOrdinal("SEMANA_EMBARAZO"))
                                    ? null
                                    : reader.GetInt32(reader.GetOrdinal("SEMANA_EMBARAZO"))
                            };
                        }
                    }

                    bool success = resultadoParam.Value != DBNull.Value && (bool)resultadoParam.Value;
                    int? codigoError = codigoErrorParam.Value != DBNull.Value ? (int?)codigoErrorParam.Value : null;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;

                    return (success, codigoError, detalleError, detalleUsuario, datosUsuario);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al ejecutar el procedimiento almacenado: " + ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }



































    }
}

