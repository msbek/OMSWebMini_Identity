﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSWebService.Data;
using OMSWebService.Model;

namespace OMSWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly NORTHWNDContext _context;

        public CategoriesController(NORTHWNDContext context)
        {
            _context = context;
        }

        // GET: api/categories?include_picture=true
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(bool include_pictures = false)
        {
            string request = HttpContext.Request.Path;

            if (include_pictures)
            {
                var result = await _context.Categories.ToListAsync();
                return result;
            }
            else
            {
                var result = await _context.Categories.
                    Select(
                    c => new Category
                    {
                        CategoryName = c.CategoryName,
                        CategoryId = c.CategoryId,
                        Description = c.Description
                    }).ToListAsync();

                return result;
            }
        }
        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category item)
        {
            _context.Categories.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategories),
                new
                {
                    id = item.CategoryId,
                    CategoryName = item.CategoryName,
                    Description = item.Description
                },
                item); 
        }
    }
}