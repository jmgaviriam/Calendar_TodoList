﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListSofka.DTO;
using TodoListSofka.Models;

namespace TodoListSofka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoitemController : ControllerBase
    {
        private readonly CalendardbContext _CalendardbContext;

        public TodoitemController(CalendardbContext calendardbContext)
        {
            _CalendardbContext = calendardbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todoitem>>> GetTodoitems()
        {
            var item = await _CalendardbContext.Todoitems.ToListAsync();
            return item;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<ActionResult<IEnumerable<Todoitem>>> GetTodoitems(Guid id)
        {
            var item = await _CalendardbContext.Todoitems.Where(x => x.Id == id || x.IdCalendar == id).ToListAsync();
            return item;
        }


        [HttpPost]
        [Route("{id:Guid}")]
        public async Task<IActionResult> CreateTodoItem(Guid id, TodoitemDTO todoItemDTO)
        {

            var todoItem = new Todoitem
            {
                IdCalendar = id,
                Title = todoItemDTO.Title,
                Description = todoItemDTO.Description,
                Responsible = todoItemDTO.Responsible

            };

            await _CalendardbContext.Todoitems.AddAsync(todoItem);
            await _CalendardbContext.SaveChangesAsync();

            return Ok(todoItem);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateTodoItem(Guid id, TodoitemDTO todoItemDTO)
        {
            var todoItem = await _CalendardbContext.Todoitems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Title = todoItemDTO.Title;
            todoItem.Description = todoItemDTO.Description;
            todoItem.Responsible = todoItemDTO.Responsible;

            await _CalendardbContext.SaveChangesAsync();

            return Ok(todoItem);
        }

        [HttpPut]
        [Route("update/{id:Guid}")]
        public async Task<IActionResult> CompleteTodoItem(Guid id)
        {
            var todoItem = await _CalendardbContext.Todoitems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.IsCompleted = true;

            await _CalendardbContext.SaveChangesAsync();

            return Ok(todoItem);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            var todoItem = await _CalendardbContext.Todoitems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.State = false;

            await _CalendardbContext.SaveChangesAsync();

            return Ok(todoItem);
        }

    }
}
