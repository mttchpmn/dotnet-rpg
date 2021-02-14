using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotnetRpg.Dtos.Character;
using DotnetRpg.Models;

namespace DotnetRpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;

        private static readonly List<Character> _characters = new List<Character>()
        {
            new Character(),
            new Character {Id = 1, Name = "Frodo"}
        };

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var characters = _characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            var result = new ServiceResponse<List<GetCharacterDto>> {Data = characters};

            return result;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var character = _mapper.Map<GetCharacterDto>(_characters.FirstOrDefault(c => c.Id == id));
            var result = new ServiceResponse<GetCharacterDto>() {Data = character};

            return result;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = _characters.Max(c => c.Id) + 1;
            _characters.Add(character);
            var mappedCharacters = _characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            var result = new ServiceResponse<List<GetCharacterDto>>() {Data = mappedCharacters};

            return result;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var result = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = _characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
                character.Name = updatedCharacter.Name;
                character.Class = updatedCharacter.Class;
                character.Defense = updatedCharacter.Defense;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Strength = updatedCharacter.Strength;
    
                result.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
            }

            return result;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var result = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = _characters.First(c => c.Id == id);
                _characters.Remove(character);
                result.Data = _characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
            }

            return result;
        }
    }
}