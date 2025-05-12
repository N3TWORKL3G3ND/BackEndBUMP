using Data.Contexts;
using Domain.DTOs;
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
    public class EmbarazoRepository : IEmbarazoRepository
    {
        private readonly BumpContext _context;

        public EmbarazoRepository(BumpContext context)
        {
            _context = context;
        }



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarEmbarazoAsync(Guid sessionGuid, DateTime fechaInicio, DateTime? fechaEstimadaParto)
        {
            var query = "EXEC SP_REGISTRAR_EMBARAZO @P_SESSION_GUID, @P_FECHA_INICIO, @P_FECHA_ESTIMADA_PARTO, @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    // Parámetros de entrada
                    command.Parameters.Add(new SqlParameter("@P_SESSION_GUID", sessionGuid));
                    command.Parameters.Add(new SqlParameter("@P_FECHA_INICIO", fechaInicio));
                    command.Parameters.Add(new SqlParameter("@P_FECHA_ESTIMADA_PARTO", (object?)fechaEstimadaParto ?? DBNull.Value));

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

                    bool success = resultadoParam.Value != DBNull.Value && (bool)resultadoParam.Value;
                    int? codigoError = codigoErrorParam.Value != DBNull.Value ? (int?)codigoErrorParam.Value : null;
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



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<EmbarazoDto> ListaEmbarazos)> ListarEmbarazosAsync(Guid sessionGuid)
        {
            var query = "EXEC SP_LISTAR_EMBARAZOS @P_SESSION_GUID, @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    // Parámetros de entrada
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

                    var listaEmbarazos = new List<EmbarazoDto>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new EmbarazoDto
                            {
                                IdEmbarazo = reader.GetInt32(reader.GetOrdinal("ID_EMBARAZO")),
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("ID_USUARIO")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("FECHA_INICIO")),
                                FechaEstimadaParto = reader.GetDateTime(reader.GetOrdinal("FECHA_ESTIMADA_PARTO")),
                                Estado = reader.GetByte(reader.GetOrdinal("ESTADO")),
                                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FECHA_REGISTRO"))
                            };

                            listaEmbarazos.Add(item);
                        }
                    }

                    bool success = resultadoParam.Value != DBNull.Value && (bool)resultadoParam.Value;
                    int? codigoError = codigoErrorParam.Value != DBNull.Value ? (int?)codigoErrorParam.Value : null;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;

                    return (success, codigoError, detalleError, detalleUsuario, listaEmbarazos);
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
