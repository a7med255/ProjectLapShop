using ProjectLapShop.Bl;
using ProjectLapShop.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectLapShop.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItems _itemService;       

        public ItemsController(IItems itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Get all items from the database.
        /// </summary>
        /// <returns>A list of items.</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var items = _itemService.GetAll();
                return Ok(new ApiResponse
                {
                    Data = items,
                    StatusCode = "200",
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Data = null,
                    StatusCode = "500",
                    Errors = ex.Message
                });
            }
        }

        /// <summary>
        /// Get an item by its ID.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <returns>The requested item.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var item = _itemService.GetById(id);
                if (item == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Data = null,
                        StatusCode = "404",
                        Errors = "Item not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Data = item,
                    StatusCode = "200",
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Data = null,
                    StatusCode = "500",
                    Errors = ex.Message
                });
            }
        }

        /// <summary>
        /// Get items by category ID.
        /// </summary>
        /// <param name="categoryId">The category ID.</param>
        /// <returns>A list of items in the category.</returns>
        [HttpGet("ByCategory/{categoryId}")]
        public IActionResult GetByCategoryId(int categoryId)
        {
            try
            {
                var items = _itemService.GetAllItemsData(categoryId);
                return Ok(new ApiResponse
                {
                    Data = items,
                    StatusCode = "200",
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Data = null,
                    StatusCode = "500",
                    Errors = ex.Message
                });
            }
        }

        /// <summary>
        /// Create a new item.
        /// </summary>
        /// <param name="item">The item to create.</param>
        /// <returns>The result of the creation process.</returns>
        [HttpPost]
        public IActionResult Create([FromBody] TbItem item)
        {
            try
            {
                if (item == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Data = null,
                        StatusCode = "400",
                        Errors = "Invalid item data"
                    });
                }

                _itemService.Save(item);
                return Ok(new ApiResponse
                {
                    Data = "Item created successfully",
                    StatusCode = "200",
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Data = null,
                    StatusCode = "500",
                    Errors = ex.Message
                });
            }
        }

        /// <summary>
        /// Delete an item by its ID.
        /// </summary>
        /// <param name="id">The ID of the item to delete.</param>
        /// <returns>The result of the deletion process.</returns>
        [HttpPost("Delete")]
        public IActionResult Delete([FromBody] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Data = null,
                        StatusCode = "400",
                        Errors = "Invalid ID"
                    });
                }

                _itemService.Delete(id);
                return Ok(new ApiResponse
                {
                    Data = "Item deleted successfully",
                    StatusCode = "200",
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Data = null,
                    StatusCode = "500",
                    Errors = ex.Message
                });
            }
        }
    }
}
