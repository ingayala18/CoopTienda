using CoopTienda.AccesoDatos.Data;
using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoopTienda.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;
        private readonly ApplicationDbContext dbContext;

        public UsuariosController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext dbContext)
        {
            this.unidadTrabajo = unidadTrabajo;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "Error al bloquear/desbloquear el usuario." });
            }
            var usuario = await unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == id);
            if (usuario is null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }
            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                // Usuario está bloqueado, proceder a desbloquear
                usuario.LockoutEnd = DateTime.Now;
                await unidadTrabajo.Guardar();
                return Json(new { success = true, message = "Usuario desbloqueado exitosamente." });
            }
            else
            {
                // Usuario no está bloqueado, proceder a bloquear
                usuario.LockoutEnd = DateTime.Now.AddYears(100);
                await unidadTrabajo.Guardar();
                return Json(new { success = true, message = "Usuario bloqueado exitosamente." });
            }
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarioLista = await unidadTrabajo.UsuarioAplicacion.ObtenerTodos();
            var userRole = await dbContext.UserRoles.ToListAsync();
            var roles = await dbContext.Roles.ToListAsync();
            foreach (var usuario in usuarioLista)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;

                if (roleId != null)
                {
                    usuario.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                }
            }
            // Identificar usuario autenticado
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuarioActual = usuarioLista.FirstOrDefault(u => u.Id == userId);

            // Si es Administrador, excluirlo de la lista
            if (usuarioActual is not null && usuarioActual.Role == "Administrador")
            {
                usuarioLista = usuarioLista.Where(u => u.Id != userId).ToList();
            }

            return Json(new { data = usuarioLista });
        }
        #endregion
    }
}

