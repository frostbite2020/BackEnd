using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Entities.Context;
using Entities.Models;
using AutoMapper;
using Entities.DTO;
using Entities.TodoCategoryDto;

namespace todoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoCategoryController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public TodoCategoryController(ILoggerManager logger, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _logger = logger;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        // GET: api/TodoCategory
        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _repoWrapper.TodoCategory.GetAllTodoCategories();

                var categoriesResult = _mapper.Map<IEnumerable<TodoCategoryDTO>>(categories);
                _logger.LogInfo($"Returned All The Categories from db");
                return Ok(categoriesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with GetCategories {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("{id}", Name = "TodoCategoryById")]
        public IActionResult GetTodoCategoryById(int id)
        {
            try
            {
                var todoCategory = _repoWrapper.TodoCategory.GetTodoCategoryById(id);
                if(todoCategory == null)
                {
                    _logger.LogError($"Id {id} that you search is not found on db");
                    return NotFound();
                }
                var todoCategoryResult = _mapper.Map<TodoCategoryDTO>(todoCategory);
                _logger.LogInfo($"Returned Todo Category That have Id: {id}");
                return Ok(todoCategoryResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with GetTodoCategoryById {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodoCategory(int id, [FromBody]CategoryCreateDto todoCategory)
        {
            try
            {
                if (todoCategory == null)
                {
                    return BadRequest("Owner object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client");
                    return BadRequest("Invalid model object");
                }
                var todoCategoryEntity = _repoWrapper.TodoCategory.GetTodoCategoryById(id);
                if(todoCategoryEntity == null)
                {
                    _logger.LogError($"Owner id: {id} is not found in db");
                    return NotFound();
                }
                var a = _repoWrapper.TodoCategory.GetCategoryTitle(todoCategory.CategoryTitle);
                if (a != null)
                {
                    return BadRequest($"we alredy have category {todoCategory.CategoryTitle} in our db, cannot have same category");
                }
                if (_repoWrapper.TodoList.ListsByCategory(id).Any())
                {
                    _logger.LogError($"Cannot update Category title with id: {id}. It has related todo list. Delete those List first");
                    return BadRequest("Cannot update todo list. It has related todo list. Delete those todo list first");
                }
                _mapper.Map(todoCategory, todoCategoryEntity);
                _repoWrapper.TodoCategory.UpdateTodoCategory(todoCategoryEntity);
                _repoWrapper.Save();
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogInfo($"There is something wrong with UpdateTodoCategory {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
        // POST: api/TodoCategory
        [HttpPost]
        public IActionResult CreateTodoCategory([FromBody]CategoryCreateDto todoCategory)
        {
            try
            {
                if(todoCategory == null)
                {
                    _logger.LogError($"TodoCategory Object Cannot be Null");
                    return BadRequest("TodoCategory Object Cannot Be Null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"TodoCategory object you input is not valid");
                    return BadRequest("TodoCategory object from client is not valid");
                }
                var a = _repoWrapper.TodoCategory.GetCategoryTitle(todoCategory.CategoryTitle);
                if (a != null)
                {
                    return BadRequest($"we alredy have category {todoCategory.CategoryTitle} in our db, cannot have same category");
                }
                var todoCategoryEntity = _mapper.Map<TodoCategory>(todoCategory);
                
                _repoWrapper.TodoCategory.CreateTodoCategory(todoCategoryEntity);
                _repoWrapper.Save();

                var createdTodoCategory = _mapper.Map<TodoCategoryDTO>(todoCategoryEntity);
                _logger.LogInfo($"Create new TodoCategory: {createdTodoCategory}");
                return CreatedAtRoute("TodoCategoryById", new { id = createdTodoCategory.ID }, createdTodoCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with CreateTodoCategory {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE: api/TodoCategory/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoCategory(int id)
        {
            try
            {
                var todoCategory = _repoWrapper.TodoCategory.GetTodoCategoryById(id);
                if (todoCategory == null)
                {
                    return NotFound();
                }
                if (_repoWrapper.TodoList.ListsByCategory(id).Any())
                {
                    _logger.LogError($"Cannot delete category with id: {id}. It has related Todo List. Delete those List first");
                    return BadRequest($"Cannot delete category! It has related Todo List. Delete them first");
                }
                _repoWrapper.TodoCategory.DeleteTodoCategory(todoCategory);
                _repoWrapper.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with DeleteTodoCategory {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
