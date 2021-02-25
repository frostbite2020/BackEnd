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
    [Route("api/[controller]")]
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
        [HttpGet]
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
        }
        //get by todoid
        //GET : api/TodoCategory
        [HttpGet]
        [Route("{todoCategoryID}")]
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
        //GET : api/TodoCategory
        /*[HttpGet]
        [Route("InnerJoin")]
        public IActionResult GetListWithCategory([FromQuery]TodoListParameters todoListParameters)
        {
            try
            {
                var a = _repository.TodoList.GetAllTodoLists(todoListParameters);
                var lists = _repository.TodoList.GetAllTodoLists(todoListParameters).Join(
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
                var metadata = new
                {
                    a.TotalCount,
                    a.PageSize,
                    a.CurrentPage,
                    a.TotalPages,
                    a.HasNext,
                    a.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {a.TotalCount} owners from database.");
                _logger.LogInfo($"Returned All Todo List from DB");
                return Ok(lists);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with GetAllList {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }*/

        
        [HttpGet]
        [Route("List/{id}", Name = "TodoListById")]
        public IActionResult GetTodoListById(int id)
        {
            try
            {
                var todoList = _repository.TodoList.GetTodoListById(id);
                if (todoList == null)
                {
                    _logger.LogError($"Id {id} that you search is not found on db");
                    return NotFound();
                }
                var todoListResult = _mapper.Map<TodoListDTO>(todoList);
                _logger.LogInfo($"Returned Todo Category That have Id: {id}");
                return Ok(todoListResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is something wrong with GetTodoCategoryById {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST: api/TodoCategory
        [HttpPost]
        public IActionResult CreateTodoList([FromBody]ListCreateDto todoList)
        {
            try
            {
                if (todoList == null)
                {
                    _logger.LogError($"TodoCategory Object Cannot be Null");
                    return BadRequest("TodoCategory Object Cannot Be Null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"TodoCategory object you input is not valid");
                    return BadRequest("TodoCategory object from client is not valid");
                }
                var a = _repository.TodoList.GetTodoListTitle(todoList.ActivityTitle);
                if (a != null)
                {
                    return BadRequest($"we alredy have category {todoList.ActivityTitle} in our db, cannot have same category");
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
                    return BadRequest("Owner object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client");
                    return BadRequest("Invalid model object");
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
                    return BadRequest($"we alredy have category {todoList.ActivityTitle} in our db, cannot have same category");
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
        // PUT: api/TodoCategory/TodoList/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutTodoList(int id, TodoList todoList)
        {
            var a = await _context.TodoLists.Where(x => x.ActivityTitle.Equals(todoList.ActivityTitle)).FirstOrDefaultAsync();

            if (id != todoList.TodoID)
            {
                return BadRequest();
            }
            if (a != null)
            {
                return BadRequest();
            }

            _context.Entry(todoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/
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
        // DELETE: api/TodoCategory/TodoList/5
/*        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoList>> DeleteTodoList(int id)
        {
            var a = await _context.TodoLists.Include(p => p.TodoCategory).Where(p => p.TodoID == id).FirstOrDefaultAsync();
            if (a == null)
            {
                return NotFound();
            }

            _context.TodoLists.Remove(a);
            await _context.SaveChangesAsync();

            return a;
        }

        private bool TodoListExists(long id)
        {
            return _context.TodoLists.Any(e => e.TodoID == id);
        }*/

    }
}
