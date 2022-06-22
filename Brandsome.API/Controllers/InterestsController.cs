﻿using Brandsome.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Brandsome.API.Controllers
{

    public class InterestsController : APIBaseController
    {
        private readonly IInterestsBL _interestsBL;

        public InterestsController(IInterestsBL interestsBL)
        {
            _interestsBL = interestsBL;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _interestsBL.GetCategories());
        }

        [AllowAnonymous]
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetSubcategories([FromRoute] int categoryId)
        {
            return Ok(await _interestsBL.GetSubCategories(categoryId));
        }

        [AllowAnonymous]
        [HttpGet("{subcategoryId}")]
        public async Task<IActionResult> GetServices([FromRoute] int subcategoryId)
        {
            return Ok(await _interestsBL.GetServices(subcategoryId));
        }
    }
}