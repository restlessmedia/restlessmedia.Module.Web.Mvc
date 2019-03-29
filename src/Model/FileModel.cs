using restlessmedia.Module.File;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class FileModel : FileEntity
  {
    public FileModel() { }

    public FileModel(FileEntity file)
    {
      Src = file.SystemFileName;
      FileName = file.FileName;
      Id = file.FileId;
    }

    [Display(Name = "Src")]
    public string Src { get; set; }

    [Display(Name = "File name")]
    public override string FileName { get { return base.FileName; } set { base.FileName = value; } }

    [Display(Name = "File")]
    public HttpPostedFileBase File
    {
      get
      {
        return _postedFile;
      }
      set
      {
        _postedFile = value;

        if (_postedFile.ContentLength == 0)
        {
          return;
        }

        FileName = _postedFile.FileName;
        SystemFileName = string.Concat(Guid.NewGuid().ToString(), this.Extension());
      }
    }

    [Display(Name = "File Id")]
    public new int? Id
    {
      get
      {
        return base.FileId;
      }
      set
      {
        base.FileId = value;
      }
    }

    private HttpPostedFileBase _postedFile = null;
  }
}