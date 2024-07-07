using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.Books;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _bookService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateDto request)
        {
            await _bookService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { Response = "Data succesfully created" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] BookEditDto request)
        {
            await _bookService.EditAsync(id, request);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _bookService.DeleteAsync(id);
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            return Ok(await _bookService.SearchByName(name));
        }
    }
}
