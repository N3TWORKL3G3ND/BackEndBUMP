﻿using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmbarazoRepository
    {
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarEmbarazoAsync(Guid sessionGuid, DateTime fechaInicio, DateTime? fechaEstimadaParto);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<EmbarazoDto> ListaEmbarazos)> ListarEmbarazosAsync(Guid sessionGuid);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarProgresoEmbarazoAsync(int idEmbarazo, decimal? pesoMadre, decimal? tamanoBebe, string notas);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<ProgresoEmbarazoDto> ListaProgresos)> ListarProgresosEmbarazoAsync(int idEmbarazo);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<HospitalDto> ListaHospitales)> ListarHospitalesAsync();
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarCitaAsync(Guid sessionGuid, int idHospital, DateTime fechaHoraCita, string descripcion, byte estado);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<CitaDto> ListaCitas)> ListarCitasAsync(Guid sessionGuid);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, DesplegarCitaDto? Cita)> DesplegarCitaAsync(int idCita);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<ConsejoDto> ListaConsejos)> ListarConsejosAsync(Guid sessionGuid);
    }
}
