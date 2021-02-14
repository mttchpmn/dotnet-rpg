using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Dtos.Character;
using DotnetRpg.Models;
using DotnetRpg.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace DotnetRpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var result = await _characterService.UpdateCharacter(updatedCharacter);

            if (result.Data == null) return NotFound(result);

            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var result = await _characterService.DeleteCharacter(id);
            
            if (result.Data == null) return NotFound(result);

            return Ok(result);
        }
    }
}