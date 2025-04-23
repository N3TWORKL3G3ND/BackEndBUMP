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

        public async Task<bool> ValidarLoginAsync(string correo, string contrasena)
        {
            var query = "EXEC sp_validar_login @p_correo, @p_contrasena, @resultado OUTPUT";
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@p_correo", correo));
                    command.Parameters.Add(new SqlParameter("@p_contrasena", contrasena));

                    //Agregar el parametro de salida
                    var resultadoParam = new SqlParameter("@resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(resultadoParam);

                    //Ejecutar el procedimiento almacenado
                    await command.ExecuteNonQueryAsync();

                    // Validar y retornar el resultado
                    if (resultadoParam.Value != DBNull.Value)
                    {
                        bool esValido = (bool)resultadoParam.Value;
                        return esValido;
                    }
                    else
                    {
                        throw new Exception("No se obtuvo una respuesta válida del procedimiento. ");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al ejecutar el procedimiento almacenado: " + ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
