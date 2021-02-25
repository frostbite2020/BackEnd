using AutoMapper;
using Entities.DTO;
using Entities.Models;
using Entities.TodoCategoryDto;
using Entities.TodoListDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoList
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Owner, OwnerDTO>();
            CreateMap<Account, AccountDTO>();
            CreateMap<OwnerForCreationDTO, Owner>();
            CreateMap<OwnerForUpdateDTO, Owner>();

            //TodoCategory
            CreateMap<TodoCategory, TodoCategoryDTO>();
            CreateMap<CategoryCreateDto, TodoCategory>();

            //TodoList
            CreateMap<TodoList, TodoListDTO>();
            CreateMap<ListCreateDto, TodoList>();

        }
    }
}
