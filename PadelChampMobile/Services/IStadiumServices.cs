using PadelChampMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelChampMobile.Services;

public interface IStadiumServices
{
    Task<IEnumerable<PadelStadiumViewModel>?> GetAll();
}

