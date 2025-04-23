using Application.Interfaces;
using Contracts.Requests;
using Contracts.Responses;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SesionService : ISesionService
    {
        private readonly ISesionRepository _repository;

        public SesionService(ISesionRepository repository)
        {
            _repository = repository;
        }



        public async Task<ResBase> IniciarSesionAsync(ReqIniciarSesion request)
        {
            ResBase response = new ResBase();
            response.errores = new List<string>();



            //Validaciones

            if (string.IsNullOrEmpty(request.correo))
            {
                response.detalle = "El correo electrónico del usuario no puede estar vacío.";
                response.errores.Add("El correo electrónico del usuario no puede estar vacío.");
                response.resultado = false;
                return response;
            }

            if (string.IsNullOrEmpty(request.contrasena))
            {
                response.detalle = "La contrasena del usuario no puede estar vacío.";
                response.errores.Add("La contrasena del usuario no puede estar vacío.");
                response.resultado = false;
                return response;
            }


            try
            {
                var respuesta = await _repository.ValidarLoginAsync(request.correo, request.contrasena);

                if (respuesta == null)
                {
                    response.resultado = false;
                    response.detalle = "No se obtuvo respuesta del procedimiento almacenado.";
                    response.errores.Add("No se obtuvo respuesta del procedimiento almacenado.");
                    return response;
                }

                if (respuesta)
                {
                    response.resultado = true;
                    response.detalle = "Se ha iniciado la sesion correctamente.";
                }
                else
                {
                    response.resultado = false;
                    response.errores.Add("Datos de sesion incorrectos.");
                    response.detalle = "Datos de sesion incorrectos.";
                }


            }
            catch (Exception ex)
            {
                response.resultado = false;
                response.errores.Add($"Error inesperado: {ex.GetType().Name} - {ex.Message}");
            }
            return response;
        }
    }
    
}

