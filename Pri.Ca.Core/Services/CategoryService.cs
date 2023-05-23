using Pri.Ca.Core.Entities;
using Pri.Ca.Core.Interfaces.Repositories;
using Pri.Ca.Core.Interfaces.Services;
using Pri.Ca.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ItemResultModel<Category>> AddAsync(string name)
        {

            //check if category exists
            if (_categoryRepository.GetAll().Any(c => c.Name.ToUpper().Contains(name.ToUpper())))
            {
                return new ItemResultModel<Category>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("Category exists!")
                    }
                };
            }
            Category newCategory = new Category();
            newCategory.Name = name;
            
            if(!await _categoryRepository.AddAsync(newCategory))
            {
                return new ItemResultModel<Category>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("Something went wrong...")
                    }
                };
            }
            return new ItemResultModel<Category> { IsSuccess = true };
        }

        public async Task<ItemResultModel<Category>> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if(category != null) 
            {
                return new ItemResultModel<Category>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("Category not found!")
                    }
                };
            }
            if(await _categoryRepository.DeleteAsync(category.Id))
                return new ItemResultModel<Category> { IsSuccess = true };
            else
            {
                return new ItemResultModel<Category>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("Something wernt wrong...") 
                    }
                };
            }
        }

        public async Task<ItemResultModel<Category>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if(categories.Count() == 0)
            {
                return new ItemResultModel<Category>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("No categories found...")
                    }
                };
            }
            return new ItemResultModel<Category>
            {
                Items = categories,
                IsSuccess = true
            };
        }

        public ItemResultModel<Category> GetAll()
        {
            var categories = _categoryRepository.GetAll();
            if (categories.Count() == 0)
            {
                return new ItemResultModel<Category>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("No categories found...")
                    }
                };
            }
            return new ItemResultModel<Category>
            {
                Items = categories,
                IsSuccess = true
            };
        }

        public async Task<ItemResultModel<Category>> UpdateAsync(int id, string name)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                return new ItemResultModel<Category>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("Category not found!")
                    }
                };
            }
            category.Name = name;
            if(await _categoryRepository.UpdateAsync(category))
            {
                return new ItemResultModel<Category> { IsSuccess = true };
            }
            else
            {
                return new ItemResultModel<Category>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    { new ValidationResult("Something went wrong...") }
                };
            }
            
        }

        public async Task<ItemResultModel<Category>> GetAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new ItemResultModel<Category>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("Category not found!")
                    }
                };
            }
            return new ItemResultModel<Category> 
            {
                IsSuccess = true,
                Items = new List<Category> { category }
            };
        }
    }
}
