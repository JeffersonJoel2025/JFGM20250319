using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JFGM20250319.AppWebMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JFGM20250319.AppWebMVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly Test20250319DbContext _context;

        public UsersController(Test20250319DbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public IActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Email,PasswordHash,paswordConfir,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                user.PasswordHash = CalcularHashMD5(user.PasswordHash);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email,PasswordHash,Role")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        public async Task<IActionResult> CerrarSession()
        {
            // Hola mundo
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            user.PasswordHash = CalcularHashMD5(user.PasswordHash);
            var usuarioAuth = await _context.
                Users.
                FirstOrDefaultAsync(s => s.Email == user.Email && s.PasswordHash == user.PasswordHash);
            if (usuarioAuth != null && usuarioAuth.UserId > 0 && usuarioAuth.Email == user.Email)
            {
                var claims = new[] {
            new Claim(ClaimTypes.Name, usuarioAuth.Email),
            new Claim("UserId", usuarioAuth.UserId.ToString()),
             new Claim("Username", usuarioAuth.Username),
            new Claim(ClaimTypes.Role, usuarioAuth.Role)
            };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "El email o contraseña estan incorrectos");
                return View();
            }
        }

        public async Task<IActionResult> Perfil()
        {

            var idStr = User.FindFirst("Id")?.Value;
            int id = int.Parse(idStr);
            var usuario = await _context.Users.FindAsync(id);
            return View(usuario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Perfil(int id, [Bind("UserId,Username,Email,Estatus,Rol")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }
            var usuerUpdate = await _context.Users
                 .FirstOrDefaultAsync(m => m.UserId == user.UserId);
            try
            {
                usuerUpdate.Username = user.Username;
                usuerUpdate.Email = user.Email;
                usuerUpdate.Role = user.Role;
                _context.Update(usuerUpdate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    return View(user);
                }
            }
        }

        private string CalcularHashMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "x2" convierte el byte en una cadena hexadecimal de dos caracteres.
                }
                return sb.ToString();
            }
        }
    }
}
