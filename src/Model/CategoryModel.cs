using restlessmedia.Module.Category;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class CategoryModel
  {
    public CategoryModel() { }

    public CategoryModel(CategoryEntity category)
    {
      Id = category.CategoryId;
      ParentId = category.CategoryParentId;
      Title = category.Title;
      Description = category.Description;
      Rank = category.Rank;
    }

    public static CategoryModel New(int parentId)
    {
      CategoryModel category = new CategoryModel();
      category.ParentId = parentId;
      return category;
    }

    public static IEnumerable<CategoryModel> GetList(IEnumerable<CategoryEntity> categoryList)
    {
      return categoryList.Select(x => new CategoryModel(x));
    }

    public int? Id { get; set; }

    public int? ParentId { get; set; }

    public int? Rank { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Display(Name = "Description")]
    public string Description { get; set; }

    public CategoryEntity GetCategoryEntity()
    {
      return new CategoryEntity
      {
        CategoryId = Id,
        Title = Title,
        CategoryParentId = ParentId,
        Description = Description,
        Rank = Rank
      };
    }
  }
}