using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace todoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountOwnerServerController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        public AccountOwnerServerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllOwners()
        {
            try
            {
                var owners = _repository.Owner.GetAllOwners();
                var ownersDTO = _mapper.Map<IEnumerable<OwnerDTO>>(owners);
                _logger.LogInfo($"Returned all owners from database.");
                return Ok(ownersDTO);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong with GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("{id}", Name = "OwnerById")]
        public IActionResult GetOwnerById(Guid id)
        {
            try
            {
                var ownerById = _repository.Owner.GetOwnerById(id);
                if(ownerById == null)
                {
                    _logger.LogError($"Id that u search is not found");
                    return NotFound();
                }
                else { 
                    var ownerByIdDTO = _mapper.Map<OwnerDTO>(ownerById);
                    _logger.LogInfo($"Returned Owner By Id");
                    return Ok(ownerByIdDTO);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong with GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server Error");
            }
        }
        [HttpGet]
        [Route("OwnerWithDetail{id}")]
        public IActionResult GetOwnerWithDetails(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerWithDetails(id);
                if(owner == null)
                {
                    _logger.LogError($"Owner Id {id} is Not Found!");
                    return NotFound();
                }
                else
                {
                    var ownerResult = _mapper.Map<OwnerDTO>(owner);
                    _logger.LogInfo($"Returned Owner with Details");
                    return Ok(ownerResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong with GetOwnerWithDetails {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost]
        public IActionResult CreateOwner([FromBody]OwnerForCreationDTO owner)
        {
            try
            {
                if(owner == null)
                {
                    _logger.LogError("Owner object that u sent is null");
                    return BadRequest("Owner object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Invalid owner object that u sent!");
                    return BadRequest("Invalid Model Object!");
                }
                var ownerEntity = _mapper.Map<Owner>(owner);
                _repository.Owner.CreateOwner(ownerEntity);
                _repository.Save();

                var createdOwner = _mapper.Map<OwnerDTO>(ownerEntity);
                return CreatedAtRoute("OwnerById", new { id = createdOwner.Id }, createdOwner);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong with CreateOwner {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateOwner(Guid id, [FromBody]OwnerForUpdateDTO owner)
        {
            try
            {
                if (owner == null)
                {
                    _logger.LogError($"Owner object that u input is null");
                    return BadRequest("Owner object is null ");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client");
                    return BadRequest("Invalid model object");
                }
                var ownerEntity = _repository.Owner.GetOwnerById(id);
                if(ownerEntity == null)
                {
                    _logger.LogError($"Owner id: {id} is not found in db");
                    return NotFound();
                }
                _mapper.Map(owner, ownerEntity);
                _repository.Owner.UpdateOwner(ownerEntity);
                _repository.Save();
                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with UpdateOwner {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteOwner(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);
                if(owner == null)
                {
                    return NotFound();
                }
                if (_repository.Account.AccountsByOwner(id).Any())
                {
                    _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                }
                _repository.Owner.DeleteOwner(owner);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with DeleteOwner {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}