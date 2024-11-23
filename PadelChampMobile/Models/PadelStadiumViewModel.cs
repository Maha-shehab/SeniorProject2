using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelChampMobile.Models;

public class PadelStadiumViewModel
{
    public int Id { get; set; }
    public string PlaygroundName { get; set; } = null!;
    public string Coordinates { get; set; } = null!;

}
