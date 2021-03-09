using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities.Context;
using Entities.Models;
using AutoMapper;
using Contracts;
using Entities.DTO;
using Entities.TodoListDto;
using Newtonsoft.Json;

namespace todoList.Controllers
{
    [Route("api/lists")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public TodoListController(ApplicationDBContext context, IMapper mapper, ILoggerManager logger, IRepositoryWrapper repository)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        //GET : api/TodoCategory
        /*[HttpGet]
        public IActionResult GetAllLists([FromQuery]TodoListParameters todoListParameters)
        {
            try
            {
                var lists = _repository.TodoList.GetAllTodoLists(todoListParameters);
                var metadata = new
                {
                    lists.TotalCount,
                    lists.PageSize,
                    lists.CurrentPage,
                    lists.TotalPages,
                    lists.HasNext,
                    lists.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {lists.TotalCount} owners from database.");
                var listsResult = _mapper.Map<IEnumerable<TodoListDTO>>(lists);
                _logger.LogInfo($"Returned All Todo List from DB");
                return Ok(listsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with GetAllList {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }*/
        //get by todoid
        //GET : api/TodoCategory
        [HttpGet]
        [Route("{todo-category-id}")]
        public IActionResult GetListByCategoryID(int todoCategoryID)
        {
            try
            {
                var a = _repository.TodoList.GetListByCategoryID(todoCategoryID).Join(
                    _repository.TodoCategory.GetAllTodoCategories(),
                    list => list.TodoCategoryID,
                    category => category.ID,
                    (list, category) => new
                    {
                        TodoID = list.TodoID,
                        ActivityTitle = list.ActivityTitle,
                        Priority = list.Priority,
                        Note = list.Note,
                        status = list.status,
                        TodoCategoryID = list.TodoCategoryID,
                        CategoryTitle = category.CategoryTitle
                    });
                if (a == null)
                {
                    _logger.LogError($"Id {todoCategoryID} that you search is not found on db");
                    return NotFound();
                }
                return Ok(a);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with GetAllList {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST: api/TodoCategory
        [HttpPost]
        public IActionResult CreateTodoList( [FromBody]ListCreateDto todoList)
        {
            try
            {
                
                if (todoList == null)
                {
                    _logger.LogError($"TodoCategory Object Cannot be Null");
                    return BadRequest(new { message = "TodoCategory Object Cannot Be Null" });
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"TodoCategory object you input is not valid");
                    return BadRequest(new { message = "TodoCategory object from client is not valid" });
                }
                var a = _repository.TodoList.GetTodoListTitle(todoList.ActivityTitle);
                if (a != null)
                {
                    return BadRequest(new { message = $"we alredy have category {todoList.ActivityTitle} in our db, cannot have same category" });
                }
                var todoListEntity = _mapper.Map<TodoList>(todoList);

                _repository.TodoList.CreateTodoList(todoListEntity);
                _repository.Save();

                var createdTodoList = _mapper.Map<TodoListDTO>(todoListEntity);
                _logger.LogInfo($"Create new TodoCategory: {createdTodoList}");
                return CreatedAtRoute("TodoCategoryById", new { id = createdTodoList.TodoID }, createdTodoList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with CreateTodoCategory {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodoList(int id, [FromBody]ListCreateDto todoList)
        {
            try
            {
                if (todoList == null)
                {
                    return BadRequest(new { message = "Owner object is null" });
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client");
                    return BadRequest(new { message = "Invalid model object" });
                }
                var todoListEntity = _repository.TodoList.GetTodoListById(id);
                if (todoListEntity == null)
                {
                    _logger.LogError($"Owner id: {id} is not found in db");
                    return NotFound();
                }
                var a = _repository.TodoList.GetTodoListTitle(todoList.ActivityTitle);
                if (a != null)
                {
                    return BadRequest(new { message = $"we alredy have category {todoList.ActivityTitle} in our db, cannot have same category" });
                }
                _mapper.Map(todoList, todoListEntity);
                _repository.TodoList.UpdateTodoList(todoListEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"There is something wrong with UpdateTodoCategory {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
        // DELETE: api/TodoCategory/TodoList/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoCategory(int id)
        {
            try
            {
                var todoList = _repository.TodoList.GetTodoListById(id);
                if (todoList == null)
                {
                    return NotFound();
                }
                _repository.TodoList.DeleteTodoList(todoList);
                _repository.Save();
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
