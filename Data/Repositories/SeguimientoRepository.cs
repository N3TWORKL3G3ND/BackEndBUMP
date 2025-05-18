using Data.Contexts;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Data.Repositories
{
    public class SeguimientoRepository : ISeguimientoRepository 
    {
        private readonly BumpContext _context;

        public SeguimientoRepository(BumpContext context)
        {
            _context = context;
        }



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<SintomaDto> ListaSintomas)>ListarSintomasCatalogoAsync()
        {
            var query = "EXEC SP_LISTAR_SINTOMAS_CATALOGO @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    // Parámetros de salida
                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var codigoErrorParam = new SqlParameter("@CODIGO_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var detalleErrorParam = new SqlParameter("@DETALLE_ERROR", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    var detalleUsuarioParam = new SqlParameter("@DETALLE_USUARIO", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultadoParam);
                    command.Parameters.Add(codigoErrorParam);
                    command.Parameters.Add(detalleErrorParam);
                    command.Parameters.Add(detalleUsuarioParam);

                    var listaSintomas = new List<SintomaDto>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var sintoma = new SintomaDto
                            {
                                IdSintoma = reader.GetInt32(reader.GetOrdinal("ID_SINTOMA")),
                                Nombre = reader.GetString(reader.GetOrdinal("NOMBRE"))
                            };

                            listaSintomas.Add(sintoma);
                        }
                    }

                    bool success = resultadoParam.Value != DBNull.Value && (bool)resultadoParam.Value;
                    int? codigoError = codigoErrorParam.Value != DBNull.Value ? (int?)codigoErrorParam.Value : null;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;

                    return (success, codigoError, detalleError, detalleUsuario, listaSintomas);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al ejecutar SP_LISTAR_SINTOMAS_CATALOGO: " + ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarSintomaAsync(Guid sessionGuid, int idSintoma, string? notas)
        {
            var query = "EXEC SP_REGISTRAR_SINTOMA @P_SESSION_GUID, @P_ID_SINTOMA, @P_NOTAS, @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
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
                    command.Parameters.Add(new SqlParameter("@P_ID_SINTOMA", idSintoma));
                    command.Parameters.Add(new SqlParameter("@P_NOTAS", (object?)notas ?? DBNull.Value));

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





























    }
}
