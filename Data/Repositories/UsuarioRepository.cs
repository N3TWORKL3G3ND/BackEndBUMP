using Data.Contexts;
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
    }












}

