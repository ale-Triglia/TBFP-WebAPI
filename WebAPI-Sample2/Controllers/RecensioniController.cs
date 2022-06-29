using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Sample2.Helper;
using WebAPI_Sample2.Models;

namespace WebAPI_Sample2.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecensioniController : ControllerBase
    {

        #region "--> Dichiarazioni"

        private IConfiguration _configuration;
        private Recensione recensione;

        #endregion

        #region "--> Costruttori"

        public RecensioniController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region "--> Metodi"

        [HttpGet, Route("Recensioni"), AllowAnonymous]
        [ProducesResponseType(typeof(Recensione[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<Models.UserInfo>> GetRecensioni()
        {
            try
            {
                //--> Leggo gli utenti configurati
                var c = new ORM.Context(_configuration);
                var result = c.GetRecensioni();

                //--> Restituisco la risposta
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpPost, Route("addRec"), Authorize ]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AddRecensione(Recensione R)
        {
            try
            {
                //--> Aggiungo l'utente
                var user =  Request.CurrentUser(_configuration);
                R.UserName = user.UserName;


                var c = new ORM.Context(_configuration);
               
                c.AddRecensione(R);
                

                //--> Restituisco la risposta
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };

            #endregion

        }
    }
}