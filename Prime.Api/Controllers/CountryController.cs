using BaseApi;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Data;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1,2,3,4")]
    public class CountryController : ApiBaseController
    {
        public CountryController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Countries.GetAll());
        }
        [AllowAnonymous]
        [HttpGet("GetTimeZones")]
        public async Task<IActionResult> GetTimeZones()
        {
            return Ok(await _unitOfWork.TbTimeZones.GetAllAsync());
        }
        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.Countries.GetAllAsync());
        }
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Countries.GetById(id));
        }

        [HttpGet("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest();
            else
                return Ok(await _unitOfWork.Countries.GetByIdAsync(id));
        }
        [HttpPost("Add")]
        public IActionResult Create(CountryDto dto)
        {
            Country Country = new Country()
            {
                CountryName = dto.CountryName,
                Code = dto.Code,
                Code2 = dto.Code2,
                Code3 = dto.Code3

            };
            var m = _unitOfWork.Countries.Add(Country);
            _unitOfWork.Save();
            return Ok(m);
        }
       
        [HttpPut("Update")]
        public IActionResult Update(Country Country)
        {
            var m = _unitOfWork.Countries.Update(Country);
            _unitOfWork.Save();
            return Ok(m);
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(Country Country)
        {
            _unitOfWork.Countries.Delete(Country);
            _unitOfWork.Save();
            return Ok();
        }
       
        [HttpGet("SearchByName")]
        public IEnumerable<Country> SearchByName(string name)
        {
            return (_unitOfWork.Countries.FindAll(m => m.CountryName.Contains(name)));
        }


    }
}
