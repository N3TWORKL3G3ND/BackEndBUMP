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



        


























    }

}

