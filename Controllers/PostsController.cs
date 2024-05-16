using ApiProva.DataContext;
using ApiProva.DTO;
using ApiProva.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApiContext _context;

        public PostsController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts
                                 .Include(p => p.Usuario)
                                 .ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts
                                     .Include(p => p.Usuario)
                                     .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(PostDTO postDto)
        {
            var usuario = await _context.Usuarios.FindAsync(postDto.UsuarioId);
            if (usuario == null)
            {
                return BadRequest("Usuario não encontrado");
            }

            var post = new Post
            {
                Titulo = postDto.Titulo,
                Conteudo = postDto.Conteudo,
                UsuarioId = postDto.UsuarioId
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDTO postDto)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Titulo = postDto.Titulo;
            post.Conteudo = postDto.Conteudo;
            post.UsuarioId = postDto.UsuarioId;

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
