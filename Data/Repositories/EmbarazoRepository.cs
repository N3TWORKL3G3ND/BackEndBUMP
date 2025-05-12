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



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarProgresoEmbarazoAsync(int idEmbarazo, decimal? pesoMadre, decimal? tamanoBebe, string notas)
        {
            var query = "EXEC SP_REGISTRAR_PROGRESO_EMBARAZO @P_ID_EMBARAZO, @P_PESO_MADRE, @P_TAMANO_BEBE, @P_NOTAS, @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    // Parámetros de entrada
                    command.Parameters.Add(new SqlParameter("@P_ID_EMBARAZO", idEmbarazo));
                    command.Parameters.Add(new SqlParameter("@P_PESO_MADRE", (object?)pesoMadre ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@P_TAMANO_BEBE", (object?)tamanoBebe ?? DBNull.Value));
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



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<ProgresoEmbarazoDto> ListaProgresos)> ListarProgresosEmbarazoAsync(int idEmbarazo)
        {
            var query = "EXEC SP_LISTAR_PROGRESOS_EMBARAZO @P_ID_EMBARAZO, @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    // Parámetros de entrada
                    command.Parameters.Add(new SqlParameter("@P_ID_EMBARAZO", idEmbarazo));

                    // Parámetros de salida
                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var codigoErrorParam = new SqlParameter("@CODIGO_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var detalleErrorParam = new SqlParameter("@DETALLE_ERROR", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    var detalleUsuarioParam = new SqlParameter("@DETALLE_USUARIO", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultadoParam);
                    command.Parameters.Add(codigoErrorParam);
                    command.Parameters.Add(detalleErrorParam);
                    command.Parameters.Add(detalleUsuarioParam);

                    var listaProgresos = new List<ProgresoEmbarazoDto>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new ProgresoEmbarazoDto
                            {
                                IdProgreso = reader.GetInt32(reader.GetOrdinal("ID_PROGRESO")),
                                IdEmbarazo = reader.GetInt32(reader.GetOrdinal("ID_EMBARAZO")),
                                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FECHA_REGISTRO")),
                                SemanaEmbarazo = reader.GetInt32(reader.GetOrdinal("SEMANA_EMBARAZO")),
                                PesoMadre = reader.IsDBNull(reader.GetOrdinal("PESO_MADRE")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("PESO_MADRE")),
                                TamanoBebe = reader.IsDBNull(reader.GetOrdinal("TAMAÑO_BEBE")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TAMAÑO_BEBE")),
                                Notas = reader.IsDBNull(reader.GetOrdinal("NOTAS")) ? null : reader.GetString(reader.GetOrdinal("NOTAS")),
                                Accion = reader.IsDBNull(reader.GetOrdinal("ACCION")) ? null : reader.GetString(reader.GetOrdinal("ACCION")),
                                UsuarioAccion = reader.IsDBNull(reader.GetOrdinal("USUARIO_ACCION")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("USUARIO_ACCION")),
                                DatoAnterior = reader.IsDBNull(reader.GetOrdinal("DATO_ANTERIOR")) ? null : reader.GetString(reader.GetOrdinal("DATO_ANTERIOR")),
                                FechaAccion = reader.IsDBNull(reader.GetOrdinal("FECHA_ACCION")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FECHA_ACCION"))
                            };

                            listaProgresos.Add(item);
                        }
                    }

                    bool success = resultadoParam.Value != DBNull.Value && (bool)resultadoParam.Value;
                    int? codigoError = codigoErrorParam.Value != DBNull.Value ? (int?)codigoErrorParam.Value : null;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;

                    return (success, codigoError, detalleError, detalleUsuario, listaProgresos);
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



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<HospitalDto> ListaHospitales)> ListarHospitalesAsync()
        {
            var query = "EXEC SP_LISTAR_HOSPITALES @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
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

                    var listaHospitales = new List<HospitalDto>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new HospitalDto
                            {
                                IdHospital = reader.GetInt32(reader.GetOrdinal("ID_HOSPITAL")),
                                Nombre = reader.GetString(reader.GetOrdinal("NOMBRE")),
                                Direccion = reader.IsDBNull(reader.GetOrdinal("DIRECCION")) ? null : reader.GetString(reader.GetOrdinal("DIRECCION")),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("TELEFONO")) ? null : reader.GetString(reader.GetOrdinal("TELEFONO"))
                            };

                            listaHospitales.Add(item);
                        }
                    }

                    bool success = resultadoParam.Value != DBNull.Value && (bool)resultadoParam.Value;
                    int? codigoError = codigoErrorParam.Value != DBNull.Value ? (int?)codigoErrorParam.Value : null;
                    string detalleError = detalleErrorParam.Value as string ?? string.Empty;
                    string detalleUsuario = detalleUsuarioParam.Value as string ?? string.Empty;

                    return (success, codigoError, detalleError, detalleUsuario, listaHospitales);
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



        public async Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarCitaAsync(Guid sessionGuid, int idHospital, DateTime fechaHoraCita, string descripcion, byte estado)
        {
            var query = "EXEC SP_REGISTRAR_CITA @P_SESSION_GUID, @P_ID_HOSPITAL, @P_FECHA_HORA_CITA, @P_DESCRIPCION, @P_ESTADO, @RESULTADO OUTPUT, @CODIGO_ERROR OUTPUT, @DETALLE_ERROR OUTPUT, @DETALLE_USUARIO OUTPUT";
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
                    command.Parameters.Add(new SqlParameter("@P_ID_HOSPITAL", idHospital));
                    command.Parameters.Add(new SqlParameter("@P_FECHA_HORA_CITA", fechaHoraCita));
                    command.Parameters.Add(new SqlParameter("@P_DESCRIPCION", (object?)descripcion ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@P_ESTADO", estado));

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
