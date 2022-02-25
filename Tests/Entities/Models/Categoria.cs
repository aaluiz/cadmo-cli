using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Entities.Interface;

namespace Entities.Models
{
    public class Categoria : IEntity
    {
        public int Id { get; set; }

        public bool FieldName { get; set; }
    }
}