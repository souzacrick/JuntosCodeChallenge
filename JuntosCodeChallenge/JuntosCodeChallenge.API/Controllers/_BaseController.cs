using JuntosCodeChallenge.Domain.Customer.CustomException;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JuntosCodeChallenge.API.Controllers
{
    public class _BaseController : ControllerBase
    {
        public ActionResult VerifyException(Exception e)
        {
            if (e.GetType().Equals(typeof(CustomerException)))
                return StatusCode(422, e.Message);
            if(e.GetType().Equals(typeof(ArgumentException)))
                return BadRequest(e.Message);
            else
                return StatusCode(500, e.Message);
        }
    }
}