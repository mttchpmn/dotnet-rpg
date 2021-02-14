using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotnetRpg.Data;
using DotnetRpg.Dtos.Character;
using DotnetRpg.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var characters = await _context.Characters.ToListAsync();
            var characterDtos = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            var result = new ServiceResponse<List<GetCharacterDto>> {Data = characterDtos};

            return result;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            var characterDto = _mapper.Map<GetCharacterDto>(character);
            var result = new ServiceResponse<GetCharacterDto>() {Data = characterDto};

            return result;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var character = _mapper.Map<Character>(newCharacter);

            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            
            var mappedCharacters = _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            var result = new ServiceResponse<List<GetCharacterDto>>() {Data = mappedCharacters};

            return result;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var result = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await  _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                
                character.Name = updatedCharacter.Name;
                character.Class = updatedCharacter.Class;
                character.Defense = updatedCharacter.Defense;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Strength = updatedCharacter.Strength;

                _context.Characters.Update(character);
                await _context.SaveChangesAsync();
    
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
                var character = await _context.Characters.FirstAsync(c => c.Id == id);
                
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                
                result.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
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